using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Views;
using Models.Combat;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
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

        [Inject]
        public MoveCombatantSignal MoveCombatantSignal { get; set; }

        [Inject]
        public NewFightForecastSignal FightForecastSignal { get; set; }

        public override void Execute() {
            var state = BattleViewModel.State;
            if (state == BattleUIState.Uninitialized ||
                state == BattleUIState.PhaseChanging) {
                return;
            }

            var combatant = BattleViewModel.Map.GetAtPosition(Position);
            if (state == BattleUIState.SelectingUnit) {
                if (combatant != null && combatant.Army == ArmyType.Friendly) {
                    var actions = GetActions(combatant);
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

                    var pathLength = BattleViewModel.CurrentMovementPath.Length;
                    var moveAction = new MoveAction(BattleViewModel.Map, BattleViewModel.SelectedCombatant, Position, pathLength);
                    BattleViewModel.PendingAction = moveAction;

                    MoveCombatantSignal.Dispatch(BattleViewModel.CurrentMovementPath);
                    BattleViewModel.State = BattleUIState.CombatantMoving;
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

        private HashSet<CombatActionType> GetActions(ICombatant combatant) {

            var battle = BattleViewModel.Battle;
            var map = BattleViewModel.Map;

            var results = new HashSet<CombatActionType>();
            if (battle.CanAct(combatant)) {
                results.Add(CombatActionType.Item);

                var attackableSquares = map.BreadthFirstSearch(combatant.Position, battle.GetMaxWeaponAttackRange(combatant), true);
                var attackableUnits = attackableSquares
                    .Select(square => map.GetAtPosition(square))
                    .Where(unit => unit != null && unit.Army == ArmyType.Enemy);

                if (attackableUnits.Any()) {
                    results.Add(CombatActionType.Attack);
                    results.Add(CombatActionType.Brace);

                    if (combatant.SpecialSkill.HasValue) {
                        results.Add(CombatActionType.Special);
                    }
                }

                var friendlyUnits = attackableSquares
                    .Select(square => map.GetAtPosition(square))
                    .Where(unit => unit != null && unit.Army == ArmyType.Friendly);
                if (friendlyUnits.Any()) {
                    results.Add(CombatActionType.Trade);
                }
            }

            if (battle.CanMove(combatant)) {
                results.Add(CombatActionType.Move);
            }

            return results;
        }
    }
}
