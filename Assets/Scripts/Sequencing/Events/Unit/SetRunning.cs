using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Set Running")]
    [USequencerEvent("Unit/SetRunning")]        
    [USequencerEventHideDuration]
    public class SetRunning : USEventBase {
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.Running = true;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}