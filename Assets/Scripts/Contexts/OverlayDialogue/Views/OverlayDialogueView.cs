using Models.Dialogue;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Assets.Contexts.OverlayDialogue.Views {
    [RequireComponent(typeof(Dialogue))]
    [RequireComponent(typeof(OverlayDialogueController))]
    public class OverlayDialogueView : View {
        public Signal DialogueCompleteSignal = new Signal();
        private Dialogue _dialogue;

        void Awake() {
            _dialogue = GetComponent<Dialogue>();
        }

        public void StartDialogue(Cutscene dialogue) {
            _dialogue.Model = dialogue;
            _dialogue.OnComplete += OnComplete;
            _dialogue.Begin();
        }

        private void OnComplete() {
            DialogueCompleteSignal.Dispatch();
            _dialogue.OnComplete -= OnComplete;
        }
    }
}