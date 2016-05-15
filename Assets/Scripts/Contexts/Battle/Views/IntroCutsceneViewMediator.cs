using System.Collections;
using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class IntroCutsceneViewMediator : Mediator {
        [Inject]
        public IntroCutsceneCompleteSignal CutscnCompleteSignal { get; set; }

        [Inject]
        public IntroCutsceneStartSignal CutsceneStartSignal { get; set; }

        [Inject]
        public IntroCutsceneView View { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            CutsceneStartSignal.AddListener(() => StartCoroutine(PlayCutscene()));
        }

        private IEnumerator PlayCutscene() {
            Debug.Log("Playing intro cutscene");
            yield return StartCoroutine(View.Play());
            CutscnCompleteSignal.Dispatch();
        }
    }
}