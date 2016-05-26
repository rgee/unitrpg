using Contexts.Battle.Signals.Camera;
using strange.extensions.mediation.impl;

namespace Contexts.Battle.Views {
    public class CameraViewMediator : Mediator {

        [Inject]
        public CameraView View { get; set; }

        [Inject]
        public CameraLockSignal CameraLockSignal { get; set; }

        [Inject]
        public CameraUnlockSignal CameraUnlockSignal { get; set; }

        public override void OnRegister() {
            CameraLockSignal.AddListener(View.Lock);
            CameraUnlockSignal.AddListener(View.Unlock);
        }
    }
}