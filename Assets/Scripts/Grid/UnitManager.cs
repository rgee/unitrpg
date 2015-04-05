﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Grid {
    public class UnitManager : MonoBehaviour {
        public MapGrid Grid;

		private List<Models.Unit> unitModels = new List<Models.Unit>();
        private List<GameObject> unitGameObjects = new List<GameObject>();
        private Dictionary<Vector2, GameObject> unitsByPosition = new Dictionary<Vector2, GameObject>();

        private HashSet<Grid.Unit> unmovedUnits = new HashSet<Unit>();

        private bool locked;
        private GameObject selectedUnit;
		public GameObject SelectedUnit {
			get { return selectedUnit; }
		}

		public delegate void UnitClickedEventHandler(Unit e, Vector2 gridPosition);
		public event UnitClickedEventHandler OnUnitClick;

        private Vector2? selectedGridPosition;

        public Grid.Unit GetUnitByPosition(Vector2 pos) {
            if (!unitsByPosition.ContainsKey(pos)) {
                return null;
            }
            return unitsByPosition[pos].GetComponent<Grid.Unit>();
        }

        // Use this for initialization
        void Start() {
            foreach (Transform t in transform) {
                unitGameObjects.Add(t.gameObject);

				Grid.Unit unit = t.gameObject.GetComponent<Grid.Unit>();

                Vector2 gridPos = unit.gridPosition;
                unitsByPosition.Add(gridPos, t.gameObject);

				unitModels.Add(unit.model);

                t.transform.position = Grid.GetWorldPosForGridPos(gridPos);
            }

            ResetMovedUnits(true);

            CombatEventBus.Deaths.AddListener(OnUnitDeath);
        }

        void OnDestroy() {
            CombatEventBus.Deaths.RemoveListener(OnUnitDeath);
        }

        void OnUnitDeath(Grid.Unit unit) {
            unitsByPosition.Remove(unit.gridPosition);
            unitModels.Remove(unit.model);

            GameObject gameObject = unit.gameObject;
            unitGameObjects.Remove(gameObject);
            Destroy(gameObject);

            Grid.RescanGraph();
        }

        public List<Grid.Unit> GetEnemies() {
            return unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .Where(unit => !unit.friendly)
                .ToList();
        }

        public List<Grid.Unit> GetFriendlies() {
            return unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .Where(unit => unit.friendly)
                .ToList();
        }

        public bool UnitsRemainingToMove() {
            return unmovedUnits.Count > 0;
        }

        public void ResetMovedUnits(bool friendlyTurn) {
            IEnumerable<Unit> unmovedUnitQuery = unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .Where(unit => unit.friendly == friendlyTurn);

            unmovedUnits = new HashSet<Unit>(unmovedUnitQuery);
        }

        public void Lock() {
            locked = true;
        }

        public void Unlock() {
            locked = false;
        }

        void Update() {

            if (locked) {
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                Vector2? maybeGridPos = Grid.GetMouseGridPosition();
                SelectUnit(maybeGridPos.Value);

                // If the click happened at a grid point
                if (maybeGridPos.HasValue) {
                    if (selectedUnit == null) {
                    }
                } else {
                    //ClearSelectedUnit();
                }
            }

            if (selectedUnit != null && Input.GetKeyDown(KeyCode.Escape)) {
                //ClearSelectedUnit();
            }
        }

		private void AttemptAttack(Vector2 position) {
			if (unitsByPosition.ContainsKey(position)) {
				GameObject unit = unitsByPosition[position];
			}
		}

        public Vector2? GetSelectedGridPosition() {
            return selectedGridPosition;
        }

        private void SelectUnit(Vector2 position) {
            if (unitsByPosition.ContainsKey(position)) {
                GameObject potentialUnit = unitsByPosition[position];
                Unit unitComponent = potentialUnit.GetComponent<Unit>();
				if (OnUnitClick != null) {
					OnUnitClick(unitComponent, position);
				}

                if (!unitComponent.friendly) {
                    return;
                }
            }
        }

        public void ChangeUnitPosition(GameObject unit, Vector2 position) {
            unitsByPosition.Remove(unit.GetComponent<Grid.Unit>().gridPosition);
            unitsByPosition.Add(position, unit);
            unit.GetComponent<Unit>().gridPosition = position;
        }
      
    }
}

