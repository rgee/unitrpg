using System;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;
using WellFired;

namespace Contexts.Battle.Views {
    public class IntroCutsceneView : View {
        private USSequencer _sequence;
        void Awake() {
            base.Awake();
            _sequence = GetComponent<USSequencer>();
        }
        public IEnumerator Play() {
            if (_sequence != null) {
                var sequenceFinished = false;
                _sequence.Play();

                USSequencer.PlaybackDelegate onComplete = null;
                onComplete = delegate {
                    sequenceFinished = true;
                    _sequence.PlaybackFinished -= onComplete;
                };
                _sequence.PlaybackFinished += onComplete;

                while (!sequenceFinished) {
                    yield return new WaitForEndOfFrame();
                }
            }
        } 
    }
}