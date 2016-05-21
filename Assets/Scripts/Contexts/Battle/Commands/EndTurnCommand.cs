using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class EndTurnCommand : Command {
        [Inject]
        public BattleViewState ViewState { get; set; }

        [Inject]
        public PlayerTurnCompleteSignal PlayerTurnCompleteSignal { get; set; }

        public override void Execute() {
            ViewState.State = BattleUIState.SelectingUnit;
            PlayerTurnCompleteSignal.Dispatch();
        }
    }
}