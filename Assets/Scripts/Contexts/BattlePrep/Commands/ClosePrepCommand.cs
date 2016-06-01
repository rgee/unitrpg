using Contexts.BattlePrep.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.BattlePrep.Commands {
    public class ClosePrepCommand : Command {
        [Inject]
        public TransitionCompleteSignal TransitionCompleteSignal{ get; set; }

        [Inject]
        public TransitionOutSignal TransitionOutSignal { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextView { get; set; }

        public override void Execute() {
            Retain();
            TransitionCompleteSignal.AddListener(OnPrepHidden);
            TransitionOutSignal.Dispatch();
        }

        private void OnPrepHidden() {
            TransitionCompleteSignal.RemoveListener(OnPrepHidden);
            GameObject.Destroy(ContextView);
            Release();
        }
    }
}
