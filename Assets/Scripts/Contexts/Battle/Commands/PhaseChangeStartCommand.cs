using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class PhaseChangeStartCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        public override void Execute() {
            base.Execute();
            Model.State = BattleUIState.PhaseChanging;
        }
    }
}
