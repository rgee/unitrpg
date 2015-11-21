using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Set Facing West")]
    [USequencerEvent("Unit/SetFacingWest")]        
    [USequencerEventHideDuration]
    public class SetFacingWest : USEventBase {
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.Facing = MathUtils.CardinalDirection.W;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}