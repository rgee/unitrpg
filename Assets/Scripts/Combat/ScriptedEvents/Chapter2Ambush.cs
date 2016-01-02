using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Combat;
using Combat.ScriptedEvents;
using Grid;
using UnityEngine;

namespace ScriptedEvents {
    public class Chapter2Ambush : CombatEvent {

        public new IScriptedEvent Event {
            get { return this; }
        }

        public List<SpawnableUnit> Units = new List<SpawnableUnit>();

        public override IEnumerator Play() {
            Debug.Log("Starting Chapter 2 Ambush");
            yield return StartCoroutine(SpawnUnits(Units));

            Debug.Log("Chapter 2 Ambush complete");
        }
    }
}
