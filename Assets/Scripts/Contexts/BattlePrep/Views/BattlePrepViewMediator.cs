using System.Collections;
using Contexts.BattlePrep.Models;
using Contexts.BattlePrep.Signals;
using strange.extensions.mediation.impl;

namespace Contexts.BattlePrep.Views {
    public class BattlePrepViewMediator : Mediator {
        [Inject]
        public BattlePrepView View { get; set; }

        [Inject]
        public TransitionOutSignal TransitionOutSignal { get; set; }

        [Inject]
        public TransitionInSignal TransitionInSignal { get; set; }

        [Inject]
        public ActionSelectedSignal ActionSelectedSignal { get; set; }

        [Inject]
        public TransitionCompleteSignal TransitionCompleteSignal { get; set; }

        [Inject]
        public NewBattleConfigSignal NewBattleConfigSignal { get; set; }

        public override void OnRegister() {
            View.ActionClickedSignal.AddListener(OnActionSelected);
            TransitionInSignal.AddListener(() => {
                StartCoroutine(TransitionIn());
            });

            TransitionOutSignal.AddListener(() => {
                StartCoroutine(TransitionOut());
            });

            NewBattleConfigSignal.AddListener((config) => {
                var objective = config.Objective;
                View.UpdateObjective(objective.Description);
            });
        }

        private void OnActionSelected(BattlePrepAction action) {
           ActionSelectedSignal.Dispatch(action); 
        }

        private IEnumerator TransitionIn() {
            yield return StartCoroutine(View.TransitionIn());
            TransitionCompleteSignal.Dispatch();
        }

        private IEnumerator TransitionOut() {
            yield return StartCoroutine(View.TransitionOut());
            TransitionCompleteSignal.Dispatch();
        }
    }
}