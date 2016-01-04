using System.Collections;
using Combat;
using Combat.Interactive.Rules;
using Combat.Managers;
using Combat.Props;
using UnityEngine;

namespace Assets.Combat.ScriptedEvents.Chapter2 {
    [RequireComponent(typeof(TogglableTileRule))]
    public class HouseVisit : MonoBehaviour, IScriptedEvent {
        public Chapter2Manager ChapterManager;
        public IToggleableProp House;
        public IToggleableProp NextHouse;

        public IEnumerator Play() {

            // TODO: Play the dialogue

            yield return StartCoroutine(House.Disable());

            if (NextHouse != null) {
                yield return StartCoroutine(NextHouse.Enable());
            }
        }
    }
}