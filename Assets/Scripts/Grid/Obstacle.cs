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
            Gizmos.color = new Color(1, 0, 0, 0.2f);

            var snappedPosition = _mapManager.GetSnappedWorldPosition(transform.position);
            Gizmos.DrawCube(snappedPosition, new Vector3(_mapManager.GridSize, _mapManager.GridSize, 1));
        }
    }
}