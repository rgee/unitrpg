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

        public override void Execute() {
            if (Model.State == BattleUIState.Uninitialized) {
                return;
            }

            if (Model.Battle.IsWon()) {
                Debug.Log("The battle is won.");
            } else if (Model.Battle.IsLost()) {
                Debug.Log("The battle is lost.");
            } else if (!Model.Battle.ShouldTurnEnd()) {
                Model.ResetUnitState();
                Model.State = BattleUIState.SelectingUnit;
            } else {
                PlayerTurnCompleteSignal.Dispatch();
            }
        }
    }
}
