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
        private readonly Func<bool> _triggerCondtion;
        private readonly bool _repeatable;

        private bool _triggered;

        public InteractiveTile(string id, Vector2 gridPosition, bool repeatable, Func<bool> triggerCondtion) {
            Id = id;
            GridPosition = gridPosition;
            _repeatable = repeatable;
            _triggerCondtion = triggerCondtion;
        }

        public bool CanTrigger() {
            if (_repeatable) {
                return _triggerCondtion.Invoke();
            }

            return !_triggered && _triggerCondtion.Invoke();
        }

        public void Trigger() {
            if (!CanTrigger()) {
                throw new ArgumentException("Tile cannot be triggered.");
            }

            _triggered = true;
        }
    }
}