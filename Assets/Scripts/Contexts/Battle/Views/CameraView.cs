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

        public void Lock() {
            UserControlEnabled = false;
        }

        public void Unlock() {
            UserControlEnabled = true;
        }
    }
}