using UnityEngine;

namespace Models.Combat {
    public class InteractiveTile {
        public readonly Vector2 GridPosition;
        public readonly string Id;

        public InteractiveTile(string id, Vector2 gridPosition) {
            Id = id;
            GridPosition = gridPosition;
        }
    }
}