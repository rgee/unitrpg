using System.Collections;
using Combat;
using Combat.Managers;
using Combat.Props;

namespace Assets.Combat.ScriptedEvents.Chapter2 {
    public class HouseVisit : IScriptedEvent {
        public Chapter2Manager ChapterManager;
        public Chapter2House House;

        public IEnumerator Play() {
            // Play the dialogue
            // Turn out the lights
            // Remove the interactive tile
            yield return null;
        }
    }
}