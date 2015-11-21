using UnityEngine;
using WellFired;

namespace Assets.Sequencing.Events.Dialogue {
    [USequencerFriendlyName("Interactive Dialogue")]
    [USequencerEvent("Dialogue/ShowInteractiveDialogue")]
    public class ShowDialogue : USEventBase {
        public GameObject Dialogue;
        public override void FireEvent() {
            Sequence.Pause();

            var dialogue = Dialogue.GetComponent<global::Dialogue>();
            dialogue.OnComplete += OnDialogueComplete;
            dialogue.Begin();
        }

        public override void ProcessEvent(float runningTime) {
        }

        private void OnDialogueComplete() {
            Sequence.Play();
        }
    }
}