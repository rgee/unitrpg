using System.Collections;
using UnityEngine;

namespace Combat {
    /**
     * Subclasses of CombatEvent will have access to a simplified
     * set of high-level actions that can be performed on the game world.
     */
    public abstract class CombatEvent : MonoBehaviour, IScriptedEvent {
        public abstract IEnumerator Play();

        protected IEnumerator PanCamera(Vector2 destination) {
            yield return null;
        }

        protected IEnumerator SpawnUnit() {
            yield return null;
        }

        protected IEnumerator StartDialogue() {
            yield return null;
        }
    }
}
