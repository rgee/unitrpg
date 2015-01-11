using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Grid {
    public class UnitManager : MonoBehaviour {
        public MapGrid Grid;

        private List<GameObject> units = new List<GameObject>();
        private Dictionary<Vector2, GameObject> unitsByPosition = new Dictionary<Vector2, GameObject>();

        private GameObject selectedUnit;
        private Vector2? selectedGridPosition;

        // Use this for initialization
        void Start() {
            foreach (Transform t in transform) {
                units.Add(t.gameObject);

                Vector2 gridPos = t.gameObject.GetComponent<Grid.Unit>().gridPosition;
                unitsByPosition.Add(gridPos, t.gameObject);

                GameObject tile = Grid.GetTileAt(gridPos);
                Vector3 tileCenter = tile.renderer.bounds.center;
                t.transform.position = tileCenter;
            }
        }

        void Update() {

            if (Input.GetMouseButtonDown(0)) {
                Vector2? maybeGridPos = Grid.GetMouseGridPosition();

                // If the click happened at a grid point
                if (maybeGridPos.HasValue) {

                    if (selectedUnit == null) {
                        SelectUnit(maybeGridPos.Value);
                    } else {
						Debug.Log(maybeGridPos);
                        MoveSelectedUnitTo(maybeGridPos.Value);
                    }
                } else {

                    ClearSelectedUnit();
                }
            }
        }

        private void SelectUnit(Vector2 position) {
            if (unitsByPosition.ContainsKey(position)) {
                selectedUnit = unitsByPosition[position];
                selectedGridPosition = position;

                MapTile tile = Grid.GetTileAt(position).GetComponent<MapTile>();
                tile.Select(Color.blue);
            }
        }

        private void ClearSelectedUnit() {
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
			        }
			    });
			    ClearSelectedUnit();
            }
        }
    }
}

