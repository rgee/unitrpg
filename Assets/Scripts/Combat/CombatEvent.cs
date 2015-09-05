using System.Collections;
using Grid;
using UnityEngine;
using System.Collections.Generic;

namespace Combat {
    /**
     * Subclasses of CombatEvent will have access to a simplified
     * set of high-level actions that can be performed on the game world.
     */
    public abstract class CombatEvent : MonoBehaviour, ITrigger, IScriptedEvent {
        public abstract HashSet<Vector2> Locations { get; }

        public IScriptedEvent Event {
            get { return this; }
        }

        public abstract IEnumerator Play();

        protected IEnumerator PanCamera(Vector2 destination) {
            yield return null;
        }

        protected IEnumerator SpawnUnits(IEnumerable<Unit> units) {
            var unitManager = CombatObjects.GetUnitManager();
            foreach (var unit in units) {
                var obj = unitManager.AddUnit(unit);
            }

            yield return null;
        }

        protected IEnumerator StartDialogue() {
            yield return null;
        }
    }
}
