using Assets.Contexts.FullscreeenDialogue.Signals;
using strange.extensions.mediation.impl;

namespace Assets.Contexts.FullscreeenDialogue.Views {
    public class FullscreenDialogueMediator : Mediator {
        [Inject]
        public DialogueCompleteSignal DialogueCompleteSignal { get; set; }

        [Inject]
        public FullscreenDialogueView View { get; set; }

        public override void OnRegister() {
            View.DialogueCompleteSignal.AddListener(DialogueCompleteSignal.Dispatch);
        }
    }
}