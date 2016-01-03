using System.Collections;
using Combat.Interactive.Rules;
using Combat.Props;
using UnityEngine;

namespace Combat.ScriptedEvents.Chapter2 {
    [RequireComponent(typeof(TogglableTileRule))]
    public class ClinicVisit : MonoBehaviour, IScriptedEvent {
        public Clinic Clinic;

        public IEnumerator Play() {
            // 1) Start Maelle dialogue
            // 2) Return to battle
            // 3) Spawn Maelle
            // 4) Turn out light (save energy!)
            yield return StartCoroutine(Clinic.Disable());
        }
    }
}