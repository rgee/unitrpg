using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Models.Fighting.Characters;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class ActionCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public PhaseChangeStartSignal PhaseChangeStartSignal { get; set; }

        public override void Execute() {
            if (Model.State == BattleUIState.Uninitialized) {
                return;
            }

            if (!Model.Battle.ShouldTurnEnd()) {
                Model.ResetUnitState();
                Model.State = BattleUIState.SelectingUnit;
            } else {
                var currentPhase = Model.Phase;
                var phaseOrder = new List<BattlePhase> {BattlePhase.Player, BattlePhase.Enemy, BattlePhase.Other};
                var nextPhase = currentPhase;
                var hasUnits = false;
                while (!hasUnits) {
                    nextPhase = phaseOrder[(phaseOrder.IndexOf(nextPhase) + 1)%phaseOrder.Count];
                    hasUnits = Model.Battle.GetByArmy(GetArmyType(nextPhase)).Any();
                }

                PhaseChangeStartSignal.Dispatch(nextPhase);
            }
        }

        private static ArmyType GetArmyType(BattlePhase phase) {
            switch (phase) {
                case BattlePhase.Player:
                    return ArmyType.Friendly;
                case BattlePhase.Enemy:
                    return ArmyType.Enemy;
                case BattlePhase.Other:
                    return ArmyType.Other;
                default:
                    throw new ArgumentOutOfRangeException("phase", phase, null);
            }
        }
    }
}
