using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Set Facing North")]
    [USequencerEvent("Unit/SetFacingNorth")]        
    [USequencerEventHideDuration]
    public class SetFacingNorth : USEventBase {
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.Facing = MathUtils.CardinalDirection.N;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}