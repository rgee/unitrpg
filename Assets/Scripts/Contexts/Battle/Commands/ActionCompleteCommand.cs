using System;
using Contexts.Battle.Models;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class ActionCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        public override void Execute() {

            if (!ShouldTurnEnd()) {
                Model.ResetUnitState();
                Model.State = BattleUIState.SelectingUnit;
            } else {
                
            }
        }

        private bool ShouldTurnEnd() {
            // TODO: Figure out whether the turn needs to end.
            return false;
        }
    }
}
