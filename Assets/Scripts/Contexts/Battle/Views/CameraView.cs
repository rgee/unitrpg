using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class CameraView : View {
        public float Speed = 300;
        public bool UserControlEnabled = true;

        public void Update() {
            if (UserControlEnabled) {
                var dt = Time.deltaTime;
                var hSpeed = Input.GetAxis("Horizontal")*Speed*dt;
                var vSpeed = Input.GetAxis("Vertical")*Speed*dt;
                var adjustment = new Vector3(hSpeed, vSpeed, 0);

                transform.Translate(adjustment);
            }
        }
    }
}