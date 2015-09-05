using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Xsl;
using UnityEngine;

namespace Grid {
    public class UnitManager : MonoBehaviour {
        public delegate void UnitClickedEventHandler(Unit e, Vector2 gridPosition, bool rightClick);

        private readonly List<GameObject> unitGameObjects = new List<GameObject>();
        private readonly List<Models.Combat.Unit> unitModels = new List<Models.Combat.Unit>();
        private readonly Dictionary<Vector2, GameObject> unitsByPosition = new Dictionary<Vector2, GameObject>();
        private MapGrid Grid;
        private bool locked;
        private Vector2? selectedGridPosition;
        private HashSet<Unit> unmovedUnits = new HashSet<Unit>();
        public GameObject SelectedUnit { get; private set; }
        public event UnitClickedEventHandler OnUnitClick;

        public Unit GetUnitByPosition(Vector2 pos) {
            if (!unitsByPosition.ContainsKey(pos)) {
                return null;
            }
            return unitsByPosition[pos].GetComponent<Unit>();
        }

        // Use this for initialization
        private void Start() {
            Grid = CombatObjects.GetMap();
            foreach (Transform t in transform) {
                AddUnit(t.gameObject);
            }

            ResetMovedUnits(true);

            CombatEventBus.Deaths.AddListener(OnUnitDeath);
            CombatEventBus.Moves.AddListener(ChangeUnitPosition);
        }

        public GameObject AddUnit(Models.Combat.Unit model) {
            var gameObject = new GameObject();
            var unitComponent = gameObject.AddComponent<Unit>();
            unitComponent.model = model;
            AddUnit(gameObject);

            return gameObject;
        }

        public void AddUnit(GameObject obj) {
            var component = obj.GetComponent<Unit>();
            if (component == null) {
                throw new ArgumentException("Provided gameobject does not have a Unit component.");
            }

            unitGameObjects.Add(obj);
            unitsByPosition.Add(component.gridPosition, obj);
            unitModels.Add(component.model);
            obj.transform.position = Grid.GetWorldPosForGridPos(component.gridPosition);
        }

        private void OnDestroy() {
            CombatEventBus.Deaths.RemoveListener(OnUnitDeath);
            CombatEventBus.Moves.RemoveListener(ChangeUnitPosition);
        }

        private void OnUnitDeath(Unit unit) {
            unitsByPosition.Remove(unit.gridPosition);
            unitModels.Remove(unit.model);

            var gameObject = unit.gameObject;
            unitGameObjects.Remove(gameObject);
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;
      
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            var executor = GameObject.FindGameObjectWithTag("Fight Executor");
            if (executor != null) {
                Destroy(executor);
            }
            Destroy(gameObject, 5);

            Grid.RescanGraph();
            CombatEventBus.ModelDeaths.Dispatch(unit.model);
        }

        public List<Unit> GetAllUnits() {
            return unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .ToList();
        }

        public List<Unit> GetEnemies() {
            return unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .Where(unit => !unit.model.IsFriendly)
                .ToList();
        }

        public List<Unit> GetFriendlies() {
            return unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .Where(unit => unit.model.IsFriendly)
                .ToList();
        }

        public bool UnitsRemainingToMove() {
            return unmovedUnits.Count > 0;
        }

        public void ResetMovedUnits(bool friendlyTurn) {
            var unmovedUnitQuery = unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .Where(unit => unit.model.IsFriendly == friendlyTurn);

            unmovedUnits = new HashSet<Unit>(unmovedUnitQuery);
        }

        public void Lock() {
            locked = true;
        }

        public void Unlock() {
            locked = false;
        }

        private void Update() {
            if (locked) {
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                var maybeGridPos = Grid.GetMouseGridPosition();
                SelectUnit(maybeGridPos.Value, false);
            } else if (Input.GetMouseButtonDown(1)) {
                var maybeGridPos = Grid.GetMouseGridPosition();
                SelectUnit(maybeGridPos.Value, true);
            }
        }

        private void AttemptAttack(Vector2 position) {
            if (unitsByPosition.ContainsKey(position)) {
                var unit = unitsByPosition[position];
            }
        }

        public Vector2? GetSelectedGridPosition() {
            return selectedGridPosition;
        }

        private void SelectUnit(Vector2 position, bool rightClick) {
            if (unitsByPosition.ContainsKey(position)) {
                var potentialUnit = unitsByPosition[position];
                var unitComponent = potentialUnit.GetComponent<Unit>();
                if (OnUnitClick != null) {
                    OnUnitClick(unitComponent, position, rightClick);
                }

                if (!unitComponent.friendly) {
                }
            }
        }

        private void ChangeUnitPosition(Unit unit, Vector2 position) {
            unitsByPosition.Remove(unit.gridPosition);
            unitsByPosition.Add(position, unit.gameObject);
            unit.GetComponent<Unit>().gridPosition = position;
        }
    }
}