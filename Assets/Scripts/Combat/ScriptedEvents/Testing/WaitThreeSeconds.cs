using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Combat.ScriptedEvents.Testing {
    public class WaitThreeSeconds : CombatEvent {
        public override IEnumerator Play() {
            yield return new WaitForSeconds(3);
        }
    }
}
