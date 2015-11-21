using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Set Facing South")]
    [USequencerEvent("Unit/SetFacingSouth")]        
    [USequencerEventHideDuration]
    public class SetFacingSOuth : USEventBase {
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.Facing = MathUtils.CardinalDirection.S;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}