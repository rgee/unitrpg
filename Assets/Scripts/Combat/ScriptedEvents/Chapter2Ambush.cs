using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Combat;
using Grid;
using UnityEngine;

namespace ScriptedEvents {
    public class Chapter2Ambush : CombatEvent {
        public override HashSet<Vector2> Locations {
            get {  return new HashSet<Vector2> { new Vector2(26, 9) }; }
        }

        public new IScriptedEvent Event {
            get { return this; }
        }

        public List<Models.Combat.Unit> units = new List<Models.Combat.Unit>();

        public override IEnumerator Play() {
            Debug.Log("Starting Chapter 2 Ambush");
            yield return new WaitForSeconds(2);

            Debug.Log("Chapter 2 Ambush complete");
        }
    }
}
