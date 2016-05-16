using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class NextPhaseCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public PhaseChangeStartSignal PhaseChangeStartSignal { get; set; }

        public override void Execute() {
            var nextPhase = Model.SelectNextPhase();
            PhaseChangeStartSignal.Dispatch(nextPhase);
        }
    }
}
