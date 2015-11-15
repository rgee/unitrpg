using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Cutscene.Views {
    public class CutsceneView : View {
        private Dialogue _dialogue;

        private void Awake() {
            _dialogue = transform.FindChild("Dialogue").GetComponent<Dialogue>();
        }

        public void Initialize(Models.Dialogue.Cutscene cutscene) {
            _dialogue.Model = cutscene;
        }

        public void StartCutscene() {
           _dialogue.Begin(); 
        }
    }
}