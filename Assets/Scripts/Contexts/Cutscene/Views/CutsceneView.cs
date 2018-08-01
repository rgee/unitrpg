using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Cutscene.Views {
    public class CutsceneView : View {
        public Signal CutsceneComplete = new Signal();
        private Dialogue _dialogue;

        private void Awake() {
            _dialogue = transform.Find("Dialogue").GetComponent<Dialogue>();

            _dialogue.OnComplete += () => {
                CutsceneComplete.Dispatch();
            };
        }

        public void Initialize(Models.Dialogue.Cutscene cutscene) {
            _dialogue.Model = cutscene;
        }

        public void StartCutscene() {
           _dialogue.Begin(); 
        }
    }
}