using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class EndTurnCommand : Command {
        [Inject]
        public BattleViewState ViewState { get; set; }

        public override void Execute() {
            ViewState.State = BattleUIState.SelectingUnit;
            ViewState.Battle.EndTurn();
        }
    }
}