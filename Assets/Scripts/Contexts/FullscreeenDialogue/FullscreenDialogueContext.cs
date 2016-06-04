using Assets.Contexts.Base;
using Assets.Contexts.FullscreeenDialogue.Commands;
using Assets.Contexts.FullscreeenDialogue.Signals;
using Assets.Contexts.FullscreeenDialogue.Views;
using UnityEngine;
using DialogueCompleteSignal = Assets.Contexts.FullscreeenDialogue.Signals.DialogueCompleteSignal;

namespace Assets.Contexts.FullscreeenDialogue {
    public class FullscreenDialogueContext : BaseContext {
        public FullscreenDialogueContext(MonoBehaviour view) : base(view) {
        }

        protected override void mapBindings() {
            base.mapBindings();

            injectionBinder.Bind<DialogueCompleteSignal>().ToSingleton();

            commandBinder.Bind<DialogueCompleteSignal>()
                .To<EndSceneCommand>();

            mediationBinder.Bind<FullscreenDialogueView>()
                .To<FullscreenDialogueMediator>();
        }
    }
}