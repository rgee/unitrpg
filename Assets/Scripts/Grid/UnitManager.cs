using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Grid {
    public class UnitManager : MonoBehaviour {
        public MapGrid Grid;

		private List<Models.Unit> unitModels = new List<Models.Unit>();
        private List<GameObject> unitGameObjects = new List<GameObject>();
        private Dictionary<Vector2, GameObject> unitsByPosition = new Dictionary<Vector2, GameObject>();
        private BattleManager battleManager;

        private HashSet<Grid.Unit> unmovedUnits = new HashSet<Unit>();

        private bool locked;
        private GameObject selectedUnit;
		public GameObject SelectedUnit {
			get { return selectedUnit; }
		}

		public delegate void UnitClickedEventHandler(Unit e, Vector2 gridPosition);
		public event UnitClickedEventHandler OnUnitClick;

        private Vector2? selectedGridPosition;

        // Use this for initialization
        void Start() {
            foreach (Transform t in transform) {
                unitGameObjects.Add(t.gameObject);

				Grid.Unit unit = t.gameObject.GetComponent<Grid.Unit>();
                Vector2 gridPos = unit.gridPosition;
                unitsByPosition.Add(gridPos, t.gameObject);

				unitModels.Add(unit.model);

                GameObject tile = Grid.GetTileAt(gridPos);
                Vector3 tileCenter = tile.GetComponent<Renderer>().bounds.center;
                t.transform.position = tileCenter;
            }

            ResetMovedUnits(true);
            //battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        }

        public List<Grid.Unit> GetEnemies() {
            return unitGameObjects
                .Select(unit => unit.GetComponent<Unit>())
                .Where(unit => !unit.friendly)
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

                // If the click happened at a grid point
                if (maybeGridPos.HasValue) {
                    if (selectedUnit == null) {
                        SelectUnit(maybeGridPos.Value);
                    } else {
                        /*j
						switch (battleManager.CurrentBattleState.Value) {
						case BattleManager.BattleState.Select_Move_Target:
							MoveSelectedUnitTo(maybeGridPos.Value);
							break;
						case BattleManager.BattleState.Select_Target:
							AttemptAttack(maybeGridPos.Value);
							break;
						}
                        */
                    }
                } else {
                    ClearSelectedUnit();
                }
            }

            if (selectedUnit != null && Input.GetKeyDown(KeyCode.Escape)) {
                ClearSelectedUnit();
            }
        }

		private void AttemptAttack(Vector2 position) {
			if (unitsByPosition.ContainsKey(position)) {
				GameObject unit = unitsByPosition[position];
				battleManager.SelectTarget(unit);
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

                selectedUnit = potentialUnit;
                selectedGridPosition = position;
                unitComponent.Select();

                //MapTile tile = Grid.GetTileAt(position).GetComponent<MapTile>();
                //tile.Select(Color.blue);

                //battleManager.StartActionSelect();
            }
        }

        private void ClearSelectedUnit() {
            Unit unitComponent = selectedUnit.GetComponent<Unit>();
            unitComponent.Deselect();

            selectedUnit = null;
            
            MapTile tile = Grid.GetTileAt(selectedGridPosition.Value).GetComponent<MapTile>();
            tile.Deselect();
            selectedGridPosition = null;
        }

        public void ChangeUnitPosition(GameObject unit, Vector2 position) {
            unitsByPosition.Remove(position);
            unitsByPosition.Add(position, unit);
        }

        private void MoveSelectedUnitTo(Vector2 position) {
            if (selectedUnit == null) {
                return;
            }

            Unit unitComp = selectedUnit.GetComponent<Unit>();
            GameObject tile = Grid.GetTileAt(position);

			Vector2 selectedPosition = selectedGridPosition.Value;
            if (!tile.GetComponent<MapTile>().blocked) {
                unitComp.MoveTo(position, Grid, 
                (gotPath) => {
                    if (gotPath) {
                        battleManager.FoundPlayerPath();
                    }
                },

                (found) => {
					if (found) {
                        ChangeUnitPosition(selectedUnit, position);

						Grid.Pathfinder.Scan();
			        }
                    ClearSelectedUnit();

                    unmovedUnits.Remove(unitComp);
                    battleManager.CompletedMovement();
			    });
            }
        }
    }
}

