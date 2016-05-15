using System.Collections;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;

namespace Contexts.Battle.Views {
    public class PhaseChangeViewMediator : Mediator {
        [Inject]
        public PhaseChangeStartSignal PhaseChangeStartSignal { get; set; }

        [Inject]
        public PhaseChangeCompleteSignal PhaseChangeCompleteSignal { get; set; }

        [Inject]
        public PhaseChangeView View { get; set; }

        public override void OnRegister() {
            base.OnRegister();
            PhaseChangeStartSignal.AddListener(phase => StartCoroutine(StartPhaseChange(phase)));
        }

        private IEnumerator StartPhaseChange(BattlePhase phase) {
            yield return View.ShowPhaseChangeText(phase);
            PhaseChangeCompleteSignal.Dispatch(phase);
        }
    }
}