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
        public override Vector2 Location {
            get {  return new Vector2(26, 9); }
        }

        public new IScriptedEvent Event {
            get { return this; }
        }

        public override IEnumerator Play() {
            Debug.Log("Starting Chapter 2 Ambush");
            yield return new WaitForSeconds(2);

            Debug.Log("Chapter 2 Ambush complete");
        }
    }
}
