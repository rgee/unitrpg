using System;
using System.Collections;
using Contexts.BattlePrep.Models;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine.UI;

namespace Contexts.BattlePrep.Views {
    public class BattlePrepView : View {
        public Signal<BattlePrepAction> ActionClickedSignal = new Signal<BattlePrepAction>();
        private Text _objectiveDescription;

        void Awake() {
            _objectiveDescription = transform.FindChild("Objective/Description").GetComponent<Text>();
        }

        public void SelectAction(string name) {
            var action = (BattlePrepAction) Enum.Parse(typeof (BattlePrepAction), name);
            ActionClickedSignal.Dispatch(action);
        }

        public void UpdateObjective(string description) {
            var capped = description.ToUpper();
            _objectiveDescription.text = capped;
        }

        public IEnumerator TransitionIn() {
            yield return null;
        }

        public IEnumerator TransitionOut() {
            yield return null;
        }
    }
}