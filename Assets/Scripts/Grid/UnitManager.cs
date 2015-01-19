using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Grid {
    public class UnitManager : MonoBehaviour {
        public MapGrid Grid;

		private List<Models.Unit> unitModels = new List<Models.Unit>();
        private List<GameObject> unitGameObjects = new List<GameObject>();
        private Dictionary<Vector2, GameObject> unitsByPosition = new Dictionary<Vector2, GameObject>();

        private bool locked;
        private GameObject selectedUnit;
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
                Vector3 tileCenter = tile.renderer.bounds.center;
                t.transform.position = tileCenter;
            }
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
                        MoveSelectedUnitTo(maybeGridPos.Value);
                    }
                } else {

                    ClearSelectedUnit();
                }
            }

            if (selectedUnit != null && Input.GetKeyDown(KeyCode.Escape)) {
                ClearSelectedUnit();
            }
        }

        private void SelectUnit(Vector2 position) {
            if (unitsByPosition.ContainsKey(position)) {
                selectedUnit = unitsByPosition[position];
                selectedGridPosition = position;

                Unit unitComponent = selectedUnit.GetComponent<Unit>();
                unitComponent.Select();

                MapTile tile = Grid.GetTileAt(position).GetComponent<MapTile>();
                tile.Select(Color.blue);
            }
        }

        private void ClearSelectedUnit() {
            selectedUnit.GetComponent<Unit>().Deselect();
            selectedUnit = null;
            
            MapTile tile = Grid.GetTileAt(selectedGridPosition.Value).GetComponent<MapTile>();
            tile.Deselect();
            selectedGridPosition = null;
        }

        private void MoveSelectedUnitTo(Vector2 position) {
            if (selectedUnit == null) {
                return;
            }

            Unit unitComp = selectedUnit.GetComponent<Unit>();
            GameObject tile = Grid.GetTileAt(position);

			Vector2 selectedPosition = selectedGridPosition.Value;
            if (!tile.GetComponent<MapTile>().blocked) {
                unitComp.MoveTo(position, Grid, (found) => {
					if (found) {
                        unitsByPosition.Remove(selectedPosition);
			            unitsByPosition.Add(position, selectedUnit);

						Grid.Pathfinder.Scan();
			        }
                    ClearSelectedUnit();
			    });
            }
        }
    }
}

