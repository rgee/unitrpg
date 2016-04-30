using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapView : View {
        public Signal<Vector2> MapClicked = new Signal<Vector2>();
        public int Width;
        public int Height;

        void Awake() {
            // Get the map manager component           
        }

        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                var clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var gridPosition = GetGridPositionForWorldPosition(clickPosition);
                if (gridPosition.HasValue) {
                    MapClicked.Dispatch(gridPosition.Value);
                }
            }
        }

        private Vector2? GetGridPositionForWorldPosition(Vector3 worldPosition) {
            // use the map manager component
            return null;
        }
    }
}