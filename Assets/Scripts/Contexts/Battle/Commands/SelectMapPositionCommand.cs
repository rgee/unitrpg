using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Contexts.Battle.Views;
using Models.Combat;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps.Triggers;
using Models.Fighting.Skills;
using strange.extensions.command.impl;
using UnityEngine;
using Utils;

namespace Contexts.Battle.Commands {
    public class SelectMapPositionCommand : Command {
        [Inject]
        public BattleViewState BattleViewModel { get; set; }

        [Inject]
        public Vector2 Position { get; set; }

        [Inject]
        public UnitSelectedSignal UnitSelectedSignal { get; set; }

        [Inject]
        public AnimateActionSignal AnimateActionSignal { get; set; }

        [Inject]
        public NewFightForecastSignal FightForecastSignal { get; set; }

        [Inject]
        public EventTileInteractedSignal EventTileInteractedSignal { get; set; }

        [Inject]
        public AvailableActions AvailableActions { get; set; }

        public override void Execute() {
            var state = BattleViewModel.State;
            if (state == BattleUIState.Uninitialized ||
                state == BattleUIState.PhaseChanging ||
                state == BattleUIState.Preparations || 
                state == BattleUIState.Surveying) {
                return;
            }

            var combatant = BattleViewModel.Map.GetAtPosition(Position);
            if (state == BattleUIState.SelectingUnit) {
                if (combatant != null && combatant.Army == ArmyType.Friendly) {
                    var actions = AvailableActions.GetAvailableActionTypes(combatant);
                    if (actions.Count <= 0) {
                        return;
                    }
                    // Mark the unit at Position as selected, change the battle state.
                    BattleViewModel.SelectedCombatant = combatant;
                    BattleViewModel.State = BattleUIState.SelectingAction;
                    BattleViewModel.AvailableActions = actions;

                    var dimensions = BattleViewModel.Dimensions;
                    var worldPosition = dimensions.GetWorldPositionForGridPosition(combatant.Position);
                    UnitSelectedSignal.Dispatch(worldPosition);
                }
            } else if (state == BattleUIState.SelectingMoveLocation) {
                // Make the unit move
                if (BattleViewModel.CurrentMovementPath != null) {
                    var map = BattleViewModel.Map;
                    var path = BattleViewModel.CurrentMovementPath;
                    var positions = path.Positions.GetRange(1, path.Positions.Count - 1);

                    var action = new MoveAction(map, path.Combatant, positions.Last(), positions);
                    AnimateActionSignal.Dispatch(action);
                    BattleViewModel.State = BattleUIState.CombatantMoving;
                }

            } else if (state == BattleUIState.SelectingInteractTarget) {
                var map = BattleViewModel.Map;
                var tile = map.GetEventTile(Position);

                // Only if the player clicks on a tile that is actually interactible by clicks
                if (tile != null && tile.InteractionMode == InteractionMode.Use) {
                    EventTileInteractedSignal.Dispatch(tile);
                }
            } else if (state == BattleUIState.SelectingAttackTarget) {
                if (combatant != null && combatant.Army == ArmyType.Enemy) {
                    // Forecast the fight against this unit
                    var attacker = BattleViewModel.SelectedCombatant;
                    var battle = BattleViewModel.Battle;
                    var selectedUnitPosition = attacker.Position;
                    var distanceToTarget = MathUtils.ManhattanDistance(selectedUnitPosition, Position);
                    var map = BattleViewModel.Map;

                    SkillType skill;
                    if (BattleViewModel.SpecialAttack) {
                        if (!attacker.SpecialSkill.HasValue) {
                            throw new ArgumentException("Combatant " + attacker.Id + " has no special skill configured.");
                        }
                        skill = attacker.SpecialSkill.Value;
                    } else {
                        skill = battle.GetWeaponSkillForRange(attacker, distanceToTarget);
                    }

                    var skillDatabase = new SkillDatabase(map);
                    var forecaster = new FightForecaster(map, skillDatabase);
                    var fight = forecaster.Forecast(BattleViewModel.SelectedCombatant, combatant, skill);
                    FightForecastSignal.Dispatch(fight);
                    BattleViewModel.FightForecast = fight;
                    BattleViewModel.State = BattleUIState.ForecastingCombat;
                }
            }
        }
    }
}
