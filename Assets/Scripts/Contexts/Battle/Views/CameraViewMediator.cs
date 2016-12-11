using Contexts.Battle.Models;
using Contexts.Battle.Signals.Camera;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class CameraViewMediator : Mediator {

        [Inject]
        public CameraView View { get; set; }

        [Inject]
        public CameraLockSignal CameraLockSignal { get; set; }

        [Inject]
        public CameraUnlockSignal CameraUnlockSignal { get; set; }

        [Inject]
        public CameraPanSignal CameraPanSignal { get; set; }

        public override void OnRegister() {
            CameraLockSignal.AddListener(View.Lock);
            CameraUnlockSignal.AddListener(View.Unlock);
            CameraPanSignal.AddListener(StartCameraPan);
        }

        private void StartCameraPan(Vector3 target) {
            StartCoroutine(View.PanTo(target));
        }
    }
}