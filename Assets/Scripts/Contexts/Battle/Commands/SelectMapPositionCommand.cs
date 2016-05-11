﻿using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Views;
using Models.Combat;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class SelectMapPositionCommand : Command {
        [Inject]
        public BattleViewState BattleViewModel { get; set; }

        [Inject]
        public Vector2 Position { get; set; }

        [Inject]
        public UnitSelectedSignal UnitSelectedSignal { get; set; }

        public override void Execute() {
            var state = BattleViewModel.State;
            var combatant = BattleViewModel.Map.GetAtPosition(Position);
            if (state == BattleUIState.SelectingUnit) {
                if (combatant != null && combatant.Army == ArmyType.Friendly) {
                    // Mark the unit at Position as selected, change the battle state.
                    BattleViewModel.SelectedCombatant = combatant;
                    BattleViewModel.State = BattleUIState.SelectingAction;
                    BattleViewModel.AvailableActions = GetActions(combatant);

                    var dimensions = BattleViewModel.Dimensions;
                    var worldPosition = dimensions.GetWorldPositionForGridPosition(combatant.Position);
                    UnitSelectedSignal.Dispatch(worldPosition);
                }
            } else if (state == BattleUIState.SelectingMoveLocation) {
                // Make the unit move
            } else if (state == BattleUIState.SelectingAttackTarget) {
                // Forecast the fight against this unit
            }
        }

        private HashSet<CombatActionType> GetActions(ICombatant combatant) {

            var battle = BattleViewModel.Battle;
            var map = BattleViewModel.Map;

            // TODO: Figure out if they can really fight at this range
            var results = new HashSet<CombatActionType> {CombatActionType.Fight};


            if (battle.CanAct(combatant)) {
                results.Add(CombatActionType.Item);

                if (map.GetAdjacent(combatant.Position).Any()) {
                    results.Add(CombatActionType.Item);
                }
            }

            if (battle.CanMove(combatant)) {
                results.Add(CombatActionType.Move);
            }

            return results;
        }
    }
}
