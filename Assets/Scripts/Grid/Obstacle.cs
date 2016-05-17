using Contexts.Battle.Views;
using UnityEngine;

namespace Grid {
    [ExecuteInEditMode]
    public class Obstacle : MonoBehaviour {
        public int Width = 1;
        public int Height = 1;

        private MapView _view;

        void Awake() {
            _view = transform.parent.parent.GetComponentInParent<MapView>();
        }

        void OnDrawGizmos() {
            // Draw a red outline around the obstacle.
            Gizmos.color = new Color(1, 0, 0, 0.2f);

            var dimensions = _view.GetDimensions();
            var gridPos = dimensions.GetGridPositionForWorldPosition(transform.position);
            var snappedPosition = dimensions.GetWorldPositionForGridPosition(gridPos);
            var gizmoWidth = Width*dimensions.TileSize;
            var gizmoHeight = Height*dimensions.TileSize;

            if (Width%2 == 0) {
                snappedPosition.x = snappedPosition.x + dimensions.TileSize / 2.0f;
            }

            if (Height%2 == 0) {
                snappedPosition.y = snappedPosition.y + dimensions.TileSize/2.0f;
            }
            Gizmos.DrawCube(snappedPosition, new Vector3(gizmoWidth, gizmoHeight, 1));
        }
    }
}