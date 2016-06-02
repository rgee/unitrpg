using Assets.Contexts.OverlayDialogue.Signals;
using Assets.Contexts.OverlayDialogue.Signals.Public;
using strange.extensions.mediation.impl;

namespace Assets.Contexts.OverlayDialogue.Views {
    public class OverlayDialogueViewMediator : Mediator {
        [Inject]
        public NewCutsceneSignal NewCutsceneSignal { get; set; }

        [Inject]
        public DialogueCompleteSignal DialogueCompleteSignal { get; set; }

        [Inject]
        public OverlayDialogueView View { get; set; }

        public override void OnRegister() {
            View.DialogueCompleteSignal.AddListener(DialogueCompleteSignal.Dispatch);
            NewCutsceneSignal.AddListener(View.StartDialogue);
        }
    }
}