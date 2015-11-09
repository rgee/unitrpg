using System.Collections;
using Contexts.BattlePrep.Signals;
using Contexts.Common.Utils;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.BattlePrep.Commands {
    public class BattlePrepStartCommand : Command {
        [Inject]
        public UpdateObjectiveSignal UpdateObjectiveSignal { get; set; }

        [Inject]
        public IRoutineRunner CorRoutineRunner { get; set; }

        public override void Execute() {
            Retain();
            CorRoutineRunner.StartCoroutine(DelayedDispatch());
        }

        private IEnumerator DelayedDispatch() {
            yield return new WaitForEndOfFrame();
            UpdateObjectiveSignal.Dispatch();
            Release();
        }
    }
}
