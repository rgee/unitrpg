using Contexts.Battle.Views;
using UnityEngine;

namespace Grid {
    [ExecuteInEditMode]
    public class Obstacle : MonoBehaviour {
        private MapView _view;

        void Awake() {
            _view = transform.parent.parent.GetComponentInParent<MapView>();
        }

        void OnDrawGizmos() {
            // Draw a red outline around the obstacle.
            Gizmos.color = new Color(1, 0, 0, 0.2f);

            var gridPos = _view.GetGridPositionForWorldPosition(transform.position);
            var snappedPosition = _view.GetWorldPositionForGridPosition(gridPos);
            var size = _view.TileSize;
            Gizmos.DrawCube(snappedPosition, new Vector3(size, size, 1));
        }
    }
}