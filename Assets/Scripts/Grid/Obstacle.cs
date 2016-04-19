using UnityEngine;

namespace Grid {
    [ExecuteInEditMode]
    public class Obstacle : MonoBehaviour {
        private MapManager _mapManager;

        void Awake() {
            _mapManager = GetComponentInParent<MapManager>();
        }

        void OnDrawGizmos() {
            // Draw a red outline around the obstacle.
            Gizmos.color = Color.red;

            var snappedPosition = _mapManager.GetSnappedGridPosition(transform.position);
            Gizmos.DrawWireCube(snappedPosition, new Vector3(_mapManager.GridSize, _mapManager.GridSize, 1));
        }
    }
}