using System.Resources;
using Contexts.Common.Model;
using Contexts.Cutscene.Signals;
using strange.extensions.mediation.impl;

namespace Contexts.Cutscene.Views {
    public class CutsceneViewMediator : Mediator {
        [Inject]
        public ApplicationState State { get; set; }

        [Inject]
        public CutsceneView View { get; set; }

        [Inject]
        public StartCutsceneSignal CutsceneStartSignal { get; set; }

        [Inject]
        public CutsceneCompleteSignal CutsceneCompleteSignal { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            CutsceneStartSignal.AddOnce(() => {
                var cutscene = State.GetCurrentCutscene();
                if (cutscene != null) {
                    View.Initialize(cutscene);
                    View.StartCutscene();
                }
            });

            View.CutsceneComplete.AddOnce(() => {
                CutsceneCompleteSignal.Dispatch(); 
            });
        }
    }
}