using Assets.Contexts.Base;
using Contexts.Base.Signals;
using Contexts.Cutscene.Commands;
using Contexts.Cutscene.Signals;
using Contexts.Cutscene.Views;
using Contexts.Global.Signals;
using strange.extensions.command.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Cutscene {
    public class CutsceneContext : BaseContext {
        public CutsceneContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            // If this is running on its own, just start the dialogue immediately.
            // If it's not, it will be coming from a scene reveal transition, so
            // wait for that to finish before triggering the first text.
            ICommandBinding startBinding;
            if (this == Context.firstContext) {
                startBinding = commandBinder.GetBinding<StartSignal>()
                    .To<LoadTestCutsceneCommand>();
            } else {
                startBinding = commandBinder.Bind<ScreenRevealedSignal>();
                commandBinder.Bind<CutsceneCompleteSignal>().To<EndCutsceneCommand>();
            }

            startBinding.To<StartDialogueCommand>().InSequence();
            mediationBinder.Bind<CutsceneView>().To<CutsceneViewMediator>();

            injectionBinder.Bind<StartCutsceneSignal>().ToSingleton();
        }
    }
}