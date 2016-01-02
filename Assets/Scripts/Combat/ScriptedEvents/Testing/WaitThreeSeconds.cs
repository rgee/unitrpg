using System.Collections;
using UnityEngine;

namespace Combat.ScriptedEvents.Testing {
    public class WaitThreeSeconds : CombatEvent {
        public override IEnumerator Play() {
            yield return new WaitForSeconds(3);
        }
    }
}
