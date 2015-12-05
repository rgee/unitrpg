using Contexts.BattlePrep.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.BattlePrep.Commands {
    public class ShowPrepCommand : Command {
        [Inject]
        public TransitionInSignal TransitionInSignal { get; set; }

        public override void Execute() {
            TransitionInSignal.Dispatch();
        }
    }
}
