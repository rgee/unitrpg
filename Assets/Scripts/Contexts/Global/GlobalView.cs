using System;
using System.Collections;
using System.Linq;
using System.Text;
using Contexts.Global.Signals;
using strange.extensions.mediation.impl;
using UI;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalView : View {

        [Inject]
        public ScreenRevealedSignal ScreenRevealedSignal { get; set; }

        [Inject]
        public ScreenFadedSignal ScreenFadedSignal { get; set; }

        private FullScreenFader _fader;

        void Start() {
            base.Start();
            var fader = Resources.Load("FullScreenFader") as GameObject;
            var faderObj = Instantiate(fader);
            faderObj.SetActive(false);

            _fader = faderObj.GetComponent<FullScreenFader>();

            faderObj.transform.SetParent(transform);
            faderObj.transform.localPosition = Vector3.zero;
            DontDestroyOnLoad(transform.gameObject);
        }

        public void FadeScreen() {
            StartCoroutine(FadeAndDispatch());
        }

        public void RevealScreen() {
            StartCoroutine(RevealAndDispatch());
        }

        private IEnumerator FadeAndDispatch() {
            yield return StartCoroutine(_fader.FadeToBlack());
            ScreenFadedSignal.Dispatch();
        }

        private IEnumerator RevealAndDispatch() {
            yield return StartCoroutine(_fader.Reveal());
            ScreenRevealedSignal.Dispatch();
        }
    }
}
