using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class ActionCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public PlayerTurnCompleteSignal PlayerTurnCompleteSignal { get; set; }

        [Inject]
        public NextBattleSignal NextBattleSignal { get; set; }

        public override void Execute() {
            // Flush the move action
            var battle = Model.Battle;
            if (Model.State == BattleUIState.CombatantMoving) {
                battle.MoveCombatant(Model.CurrentMovementPath.Combatant, Model.CurrentMovementPath.Positions);
            } else if (Model.State == BattleUIState.Fighting) {
                battle.ExecuteFight(Model.FinalizedFight);
            }

            if (Model.State == BattleUIState.Uninitialized) {
                return;
            }

            if (battle.IsWon()) {
                Debug.Log("The battle is won.");
                NextBattleSignal.Dispatch();
            } else if (battle.IsLost()) {
                Debug.Log("The battle is lost.");
            } else if (!battle.ShouldTurnEnd()) {
                Model.ResetUnitState();
                Model.State = BattleUIState.SelectingUnit;
            } else {
                PlayerTurnCompleteSignal.Dispatch();
            }
        }
    }
}
