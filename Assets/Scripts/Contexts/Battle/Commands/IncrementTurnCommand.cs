using System.Collections.Generic;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class IncrementTurnCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public PlayerTurnCompleteSignal PlayerTurnCompleteSignal { get; set; }
        
        public override void Execute() {
            if (!Model.Battle.ShouldTurnEnd()) {
                Model.ResetUnitState();
                Model.State = BattleUIState.SelectingUnit;
            } else {
                PlayerTurnCompleteSignal.Dispatch();
            }
        }
    }
}
