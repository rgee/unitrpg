using System.IO;
using Assets.Contexts.Base;
using Assets.Contexts.OverlayDialogue.Commands;
using Assets.Contexts.OverlayDialogue.Signals;
using Assets.Contexts.OverlayDialogue.Signals.Public;
using Assets.Contexts.OverlayDialogue.Views;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.OverlayDialogue {
    public class OverlayDialogueContext : BaseContext {
        public OverlayDialogueContext(MonoBehaviour view) : base(view) {

        }

        public override void Launch() {
            base.Launch();

            if (this == Context.firstContext) {
                var signal = injectionBinder.GetInstance<StartDialogueSignal>();
                var endSignal = injectionBinder.GetInstance<DialogueCompleteSignal>();
                endSignal.AddListener(() => {
                    Debug.Log("Dialogue complete");
                });

                var path = Path.Combine(Path.Combine("Chapter 1", "Battle"), "intro");
                signal.Dispatch(path);
            }
        }

        protected override void mapBindings() {
            base.mapBindings();

            injectionBinder.Bind<StartDialogueSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<DialogueCompleteSignal>().ToSingleton().CrossContext();

            injectionBinder.Bind<NewCutsceneSignal>().ToSingleton();

            mediationBinder.Bind<OverlayDialogueView>().To<OverlayDialogueViewMediator>();

            commandBinder.Bind<StartDialogueSignal>().To<StartDialogueCommand>();
        }
    }
}