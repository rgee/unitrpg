using System.Collections;
using Combat;

namespace Assets.Combat.ScriptedEvents.Chapter2 {
    public class HouseVisit : IScriptedEvent {
        public IEnumerator Play() {
            // Play the dialogue
            // Turn out the lights
            // Remove the interactive tile
            yield return null;
        }
    }
}