using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerEvent("Unit/Attack")]
    [USequencerFriendlyName("Attack")]
    [USequencerEventHideDuration]
    public class SetAttacking : USEventBase {
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.Attacking = true;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}