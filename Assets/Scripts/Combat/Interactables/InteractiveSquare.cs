using System;
using System.Collections;
using UnityEngine;

namespace Combat.Interactables {
    [RequireComponent(typeof(IScriptedEvent))]
    public class InteractiveSquare : MonoBehaviour {
        [Tooltip("Whether this object has already been interacted with.")]
        public bool Triggered;

        [Tooltip("Whether this object can be interacted with repeatedly.")]
        public bool Repeatable;

        [Tooltip("The position of this square in grid space.")]
        public Vector2 GridPosition;

        [Tooltip("The name of the action that will be taken when this object is interacted with.")]
        public string ActionName;

        private IScriptedEvent _event;

        private void Awake() {
            _event = GetComponent<IScriptedEvent>();
        }

        public IEnumerator Trigger() {
            // If it's a repeatable interaction, just let it go
            // Otherwise, only activate if it hasn't already been activated, and mark that it's been activated.
            if (!Repeatable) {
                if (Triggered) {
                    yield return null;
                } else {
                    yield return StartCoroutine(_event.Play());
                    Triggered = true;
                }
            } else {
                yield return StartCoroutine(_event.Play());
            }
        }
    }
}