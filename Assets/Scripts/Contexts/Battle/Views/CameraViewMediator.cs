using System.Collections;
using Contexts.Battle.Models;
using Contexts.Battle.Signals.Camera;
using Models.Fighting.Battle;
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

        [Inject]
        public CameraPanCompleteSignal CameraPanCompleteSignal { get; set; }

        [Inject]
        public CameraPanToPointOfInterestSignal CameraPanToPointOfInterestSignal { get; set; }

        public override void OnRegister() {
            CameraLockSignal.AddListener(View.Lock);
            CameraUnlockSignal.AddListener(View.Unlock);
            CameraPanSignal.AddListener(StartCameraPan);
            CameraPanToPointOfInterestSignal.AddListener(StartCameraPan);
        }

        private void StartCameraPan(Vector3 target) {
            StartCoroutine(DoPan(target));
        }

        private void StartCameraPan(IPointOfInterest pointOfInterest) {
            var focalPoint = pointOfInterest.FocalPoint;
            var currentCameraPosition = View.transform.position;

            // If the camera is not far enough away from the point of interest, don't
            // move. We don't want to show and make the user wait for micro-movements of
            // the camera.
            var distanceFromFocalPoint = Vector3.Distance(currentCameraPosition, focalPoint);
            if (distanceFromFocalPoint <= pointOfInterest.Tolerance) {
                CameraPanCompleteSignal.Dispatch();
            } else {
                StartCoroutine(DoPan(focalPoint));
            }
        }

        private IEnumerator DoPan(Vector3 target) {
            yield return StartCoroutine(View.PanTo(target));
            CameraPanCompleteSignal.Dispatch();
        }
    }
}