﻿using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;

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
                    Model.State = BattleUIState.SelectingUnit;
                    UnitDeselectedSignal.Dispatch();
                    break;

            }
        }
    }
}
