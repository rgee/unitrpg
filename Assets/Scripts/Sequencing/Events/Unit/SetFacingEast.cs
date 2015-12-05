using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Set Facing East")]
    [USequencerEvent("Unit/SetFacingEast")]        
    [USequencerEventHideDuration]
    public class SetFacingEast : USEventBase {
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.Facing = MathUtils.CardinalDirection.E;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}