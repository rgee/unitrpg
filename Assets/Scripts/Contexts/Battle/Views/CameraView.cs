using System.Collections;
using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class CameraView : View {
        public float Speed = 300;
        public bool UserControlEnabled;

        public void Update() {
            if (!UserControlEnabled) {
                return;
            }

            var dt = Time.deltaTime;
            var hSpeed = Input.GetAxis("Horizontal")*Speed*dt;
            var vSpeed = Input.GetAxis("Vertical")*Speed*dt;
            var adjustment = new Vector3(hSpeed, vSpeed, 0);

            transform.Translate(adjustment);
        }

        public IEnumerator PanTo(Vector3 target) {
            target.z = transform.position.z;
            var tween = transform.DOMove(target, 0.3f);
            yield return tween.WaitForCompletion();
        }

        public void Lock() {
            UserControlEnabled = false;
        }

        public void Unlock() {
            UserControlEnabled = true;
        }
    }
}