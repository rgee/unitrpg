using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Models.Combat {
    /// <summary>
    /// A point on the map that can be interacted with via 'Use'. What happens at that point is up to scripting.
    /// </summary>
    public class InteractiveTile {

        public readonly Vector2 GridPosition;
        public readonly string Id;

        private readonly bool _repeatable;
        private bool _triggered;

        public InteractiveTile(string id, Vector2 gridPosition, bool repeatable) {
            Id = id;
            GridPosition = gridPosition;
            _repeatable = repeatable;
        }

        public bool CanTrigger() {
            return _repeatable || !_triggered;
        }

        public void Trigger() {
            if (!CanTrigger()) {
                throw new ArgumentException("Cannot trigger non-repeatable interactive tile twice");
            }

            _triggered = true;
        }
    }
}