using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class PhaseChangeCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public BattlePhase Phase { get; set; }

        public override void Execute() {

            if (Phase == BattlePhase.Enemy) {
                Model.State = BattleUIState.EnemyTurn;
            } else if (Phase == BattlePhase.Player) {
                Model.State = BattleUIState.SelectingUnit;
            }

            if (Model.Phase != BattlePhase.NotStarted) {
                Model.ResetUnitState();
                Model.Battle.EndTurn();
            }

            Model.Phase = Phase;
        }
    }
}
