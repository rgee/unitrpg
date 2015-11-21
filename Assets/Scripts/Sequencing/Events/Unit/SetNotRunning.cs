using WellFired;

namespace Assets.Sequencing.Events.Unit {
    [USequencerFriendlyName("Set Not Running")]
    [USequencerEvent("Unit/SetNotRunning")]        
    [USequencerEventHideDuration]
    public class SetNotRunning : USEventBase{
         
        public override void FireEvent() {
            var unit = AffectedObject.GetComponent<Grid.Unit>();
            unit.Running = false;
        }

        public override void ProcessEvent(float runningTime) {
        }
    }
}