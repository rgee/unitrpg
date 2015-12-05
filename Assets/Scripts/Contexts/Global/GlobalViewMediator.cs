using Contexts.Base.Signals;
using Contexts.Global.Signals;
using strange.extensions.mediation.impl;

namespace Contexts.Global {
    public class GlobalViewMediator : Mediator {
        [Inject]
        public RevealScreenSignal RevealScreenSignal { get; set; }

        [Inject]
        public FadeScreenSignal FadeScreenSignal { get; set; }

        [Inject]
        public GlobalView View { get; set; }

        public override void OnRegister() {
            FadeScreenSignal.AddListener(FadeScene); 
            RevealScreenSignal.AddListener(RevealScreen);
        }

        private void FadeScene() {
            View.FadeScreen();
        }

        private void RevealScreen() {
           View.RevealScreen(); 
        }
    }
}