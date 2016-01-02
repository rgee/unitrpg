using System.Collections;
using System.Collections.Generic;

namespace Combat.ScriptedEvents.Testing {
    public class SpawnSingleUnit : CombatEvent {
        public SpawnableUnit Unit;

        public override IEnumerator Play() {
            var units = new List<SpawnableUnit>() { Unit };
            ScheduleReinforcements(units);
            yield return null;
        }
    }
}