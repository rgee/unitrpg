using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class PhaseChangeCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public BattlePhase Phase { get; set; }

        public override void Execute() {
            if (Model.Battle.TurnNumber > 0) {
                Model.ResetUnitState();
                Model.Battle.EndTurn();
            }

            if (Phase == BattlePhase.Enemy) {
                Model.State = BattleUIState.EnemyTurn;
            } else if (Phase == BattlePhase.Player) {
                Model.State = BattleUIState.SelectingUnit;
            }
        }
    }
}
