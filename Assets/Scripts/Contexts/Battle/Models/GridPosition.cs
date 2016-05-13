using UnityEngine;

namespace Contexts.Battle.Models {
    public class GridPosition {
        public readonly Vector2 GridCoordinates;
        public readonly Vector3 WorldCoordinates;

        public GridPosition(Vector2 gridCoordinates, Vector3 worldCoordinates) {
            GridCoordinates = gridCoordinates;
            WorldCoordinates = worldCoordinates;
        }
    }
}