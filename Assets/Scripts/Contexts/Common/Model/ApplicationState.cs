using System.Collections.Generic;
using UnityEngine;

namespace Contexts.Common.Model {
    public class ApplicationState {
        public readonly Stack<string> AdditionalSceneStack = new Stack<string>();
        public List<Models.Dialogue.Cutscene> CurrentCutsceneSequence { get; set; }
        private int _currentCutsceneIndex;

        public Models.Dialogue.Cutscene GetCurrentCutscene() {
            if (CurrentCutsceneSequence.Count <= 0 || CurrentCutsceneSequence.Count <= _currentCutsceneIndex) {
                return null;
            }

            return CurrentCutsceneSequence[_currentCutsceneIndex];
        }

        public void EndCurrentCutscene() {
            _currentCutsceneIndex++;
        }
    }
}