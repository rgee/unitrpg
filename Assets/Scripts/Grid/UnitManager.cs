using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Grid {
    public class UnitManager : MonoBehaviour {
        public MapGrid Grid;

        private List<GameObject> units = new List<GameObject>();
        private Dictionary<Vector2, GameObject> unitsByPosition = new Dictionary<Vector2, GameObject>();

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
                if (maybeGridPos.HasValue) {
                    Vector2 gridPos = maybeGridPos.Value;
 
                    GameObject tile = Grid.GetTileAt(gridPos);

                    // Do not move to blocked tiles.
                    if (!tile.GetComponent<MapTile>().blocked) {
                        foreach (GameObject unit in unitsByPosition.Values) {
                            unit.transform.position = tile.renderer.bounds.center;
                        }
                    }
                }
            }
        }
    }
}

