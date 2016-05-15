using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class PhaseChangeStartCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public BattlePhase NextPhase { get; set; }

        public override void Execute() {
            Model.Phase = NextPhase;
            Model.State = BattleUIState.PhaseChanging;
        }
    }
}
