using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalView : View {

        void Start() {
            base.Start();
            var fader = Resources.Load("FullScreenFader") as GameObject;
            var faderObj = Instantiate(fader);

            faderObj.transform.SetParent(transform);
            faderObj.transform.localPosition = Vector3.zero;
        }
    }
}
