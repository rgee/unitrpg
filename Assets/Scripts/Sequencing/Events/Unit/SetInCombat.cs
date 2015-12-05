using System;
using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Set Combat")]
    [USequencerEvent("Unit/SetCombat")]        
    [USequencerEventHideDuration]
    public class SetInCombat : USEventBase {
        public bool InCombat = true;
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.InCombat = InCombat;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}