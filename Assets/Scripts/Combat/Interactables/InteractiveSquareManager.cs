using System.Collections.Generic;
using UnityEngine;

namespace Combat.Interactables {
    public class InteractiveSquareManager : MonoBehaviour {
        private readonly Dictionary<Vector2, InteractiveSquare> _interactiveSquaresByPosition = new Dictionary<Vector2, InteractiveSquare>();

        void Awake() {
            var squares = GetComponentsInChildren<InteractiveSquare>();
            foreach (var square in squares) {
                _interactiveSquaresByPosition[square.GridPosition] = square;
            }
        }

        public InteractiveSquare GetInteractiveSquare(Models.Combat.Unit unit) {
            var position = unit.GridPosition;
            foreach (var neighbor in MathUtils.GetAdjacentPoints(position)) {
                if (_interactiveSquaresByPosition.ContainsKey(neighbor)) {
                    return _interactiveSquaresByPosition[position];
                }
            }

            return null;
        }
    }
}