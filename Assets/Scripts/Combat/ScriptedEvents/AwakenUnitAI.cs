using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.ScriptedEvents {
    public class AwakenUnitAI : CombatEvent {
        public List<GameObject> UnitsToEnable; 

        public override IEnumerator Play() {
            foreach (var unit in UnitsToEnable) {
                var ai = unit.GetComponent(typeof (AIStrategy)) as AIStrategy;
                if (ai != null) {
                    ai.Awake = true;
                }
            }
            yield return null;
        }
    }
}