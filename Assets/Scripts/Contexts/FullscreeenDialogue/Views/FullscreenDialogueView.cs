using Models.Dialogue;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Assets.Contexts.FullscreeenDialogue.Views {
    [RequireComponent(typeof(Dialogue))]
    [RequireComponent(typeof(FullscreenDialogueController))]
    public class FullscreenDialogueView : View {
        public Signal DialogueCompleteSignal = new Signal();
        private Dialogue _dialogue;

        void Awake() {
            _dialogue = GetComponent<Dialogue>();
            _dialogue.OnComplete += OnComplete;
        }

        private void OnComplete() {
            DialogueCompleteSignal.Dispatch();
            _dialogue.OnComplete -= OnComplete;
        }
    }
}