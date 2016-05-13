using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;
using UnityEditor;

namespace Contexts.Battle.Commands {
    public class BackCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public UnitDeselectedSignal UnitDeselectedSignal { get; set; }

        public override void Execute() {
            var state = Model.State;

            switch (state) {
                case BattleUIState.SelectingAction:
                    Model.SelectedCombatant = null;
                    UnitDeselectedSignal.Dispatch();
                    Model.State = BattleUIState.SelectingUnit;
                    break;
                case BattleUIState.SelectingFightAction:
                    Model.State = BattleUIState.SelectingAction;
                    break;

                case BattleUIState.SelectingMoveLocation:
                    Model.State = BattleUIState.SelectingAction;
                    break;
            }
        }
    }
}
