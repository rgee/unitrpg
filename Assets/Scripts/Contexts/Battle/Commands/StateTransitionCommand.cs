using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Signals.Camera;
using Contexts.Battle.Utilities;
using Models.Fighting.Characters;
using Models.Fighting.Maps;
using Models.Fighting.Maps.Triggers;
using strange.extensions.command.impl;
using UnityEngine;
using Utils;
using Attribute = Models.Fighting.Attribute;

namespace Contexts.Battle.Commands {
    public class StateTransitionCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public StateTransition Transition { get; set; }

        [Inject]
        public MovementPathUnavailableSignal PathUnavailableSignal { get; set; }

        [Inject]
        public HoverTileDisableSignal HoverTileDisableSignal { get; set; }

        [Inject]
        public UnitDeselectedSignal UnitDeselectedSignal { get; set; }

        [Inject]
        public ClearHighlightSignal ClearHighlightSignal { get; set; }

        [Inject]
        public NewMapHighlightSignal HighlightSignal { get; set; }

        [Inject]
        public FightForecastDisableSignal FightForecastDisableSignal { get; set; }

        [Inject]
        public HoverTileEnableSignal HoverTileEnableSignal { get; set; }

        [Inject]
        public CameraLockSignal CameraLockSignal { get; set; }

        [Inject]
        public CameraUnlockSignal CameraUnlockSignal { get; set; }

        [Inject]
        public EnemyTurnStartSignal EnemyTurnStartSignal { get; set; }

        [Inject]
        public AnimateActionSignal AnimateActionSignal { get; set; }

        public override void Execute() {
            Debug.LogFormat("State Transition from {0} to {1}", Transition.Previous, Transition.Next);
            Cleanup(Transition.Previous);
            Setup(Transition.Next);
        }

        private void Cleanup(BattleUIState state) {
            switch (state) {
                case BattleUIState.SelectingUnit:
                    break;
                case BattleUIState.SelectingAction:
                    HoverTileEnableSignal.Dispatch();
                    UnitDeselectedSignal.Dispatch();
                    break;
                case BattleUIState.SelectingFightAction:
                    break;
                case BattleUIState.SelectingAttackTarget:
                    ClearHighlightSignal.Dispatch(HighlightLevel.PlayerAttack);
                    break;
                case BattleUIState.SelectingInteractTarget:
                    ClearHighlightSignal.Dispatch(HighlightLevel.PlayerInteract);
                    break;
                case BattleUIState.SelectingMoveLocation:
                    ClearHighlightSignal.Dispatch(HighlightLevel.PlayerAttack);
                    ClearHighlightSignal.Dispatch(HighlightLevel.PlayerMove);
                    PathUnavailableSignal.Dispatch();
                    break;
                case BattleUIState.Fighting:
                    HoverTileEnableSignal.Dispatch();
                    break;
                case BattleUIState.CombatantMoving:
                    break;
                case BattleUIState.ForecastingCombat:
                    FightForecastDisableSignal.Dispatch();
                    break;
                case BattleUIState.Uninitialized:
                    break;
                case BattleUIState.PhaseChanging:
                    HoverTileEnableSignal.Dispatch();
                    CameraUnlockSignal.Dispatch();
                    break;
                case BattleUIState.EnemyTurn:
                    HoverTileEnableSignal.Dispatch();
                    CameraUnlockSignal.Dispatch();
                    break;
                case BattleUIState.ContextMenu:
                    break;
                case BattleUIState.Preparations:
                    break;
                case BattleUIState.Surveying:
                    break;
                case BattleUIState.EventPlaying:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        private void Setup(BattleUIState state) {
            switch (state) {
                case BattleUIState.CombatantMoving:
                    break;
                case BattleUIState.SelectingUnit:
                    HoverTileEnableSignal.Dispatch();
                    break;
                case BattleUIState.SelectingAction:
                    HoverTileDisableSignal.Dispatch();
                    break;
                case BattleUIState.ForecastingCombat:
                    break;
                case BattleUIState.SelectingFightAction:
                    break;
                case BattleUIState.SelectingAttackTarget:
                    SetupAttackTargetState();
                    break;
                case BattleUIState.SelectingInteractTarget:
                    SetupInteractTargetState();
                    break;
                case BattleUIState.SelectingMoveLocation:
                    SetupMoveLocationState();
                    break;
                case BattleUIState.Fighting:
                    HoverTileDisableSignal.Dispatch();
                    AnimateActionSignal.Dispatch(Model.PendingAction);
                    break;
                case BattleUIState.Uninitialized:
                    break;
                case BattleUIState.PhaseChanging:
                    HoverTileDisableSignal.Dispatch();
                    CameraLockSignal.Dispatch();
                    break;
                case BattleUIState.EnemyTurn:
                    CameraLockSignal.Dispatch();
                    HoverTileDisableSignal.Dispatch();
                    EnemyTurnStartSignal.Dispatch();
                    break;
                case BattleUIState.ContextMenu:
                    HoverTileDisableSignal.Dispatch();
                    break;
                case BattleUIState.Preparations:
                    CameraLockSignal.Dispatch();
                    break;
                case BattleUIState.Surveying:
                    CameraUnlockSignal.Dispatch();
                    break;
                case BattleUIState.EventPlaying:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        private void SetupInteractTargetState() {
            var origin = Model.SelectedCombatant.Position;
            var interactiveSquares = Model.Map.GetEventTilesSurrounding(origin)
                .Where(tile => tile.InteractionMode == InteractionMode.Use)
                .Select(tile => tile.Location)
                .ToHashSet();
            var highlights = new MapHighlights(interactiveSquares, HighlightLevel.PlayerInteract);

            HighlightSignal.Dispatch(highlights);
            HoverTileDisableSignal.Dispatch();
        }

        private void SetupAttackTargetState() {
            var map = Model.Map;
            var battle = Model.Battle;
            var origin = Model.SelectedCombatant.Position;
            var range = battle.GetMaxWeaponAttackRange(Model.SelectedCombatant);
            var attackableSquares = map.FindSurroundingPoints(origin, range);
            var highlights = new MapHighlights(attackableSquares, HighlightLevel.PlayerAttack);

            HighlightSignal.Dispatch(highlights);
            HoverTileDisableSignal.Dispatch();
        }

        private void SetupMoveLocationState() {
            var map = Model.Map;
            var origin = Model.SelectedCombatant.Position;
            var moveRange = Model.Battle.GetRemainingMoves(Model.SelectedCombatant);
            Predicate<KeyValuePair<Vector2, Tile>> movabilityPredicate = pair => {
                // If the tile is obstructed by props don't let them in.
                var tile = pair.Value;
                if (tile.Obstructed) {
                    return false;
                }

                // If the occupant is friendly, and they are NOT at a terminal point, let them in.
                var location = pair.Key;
                var occupant = tile.Occupant;
                if (occupant != null && !occupant.Army.IsEnemyOf(Model.SelectedCombatant.Army)) {
                    var distance = MathUtils.ManhattanDistance(origin, location);
                    if (distance == moveRange) {
                        return false;
                    }
                }

                return true;
            };

            
            var squares = map.RangeQuery(origin, moveRange, movabilityPredicate)
                .Select(square => map.FindPath(origin, square))

                // If the length of the path to the square (minus the start point) is within range, let it in
                .Where(path => path != null && path.Count() - 1 <= moveRange)

                // Collect all points along the path to make sure we highlight squares that were not themselves
                // standable (due to being obstructed by a path, but can be walked-through becuase of the
                // friendly unit passthru rule.
                .SelectMany(path => path.Skip(1).ToList())
                .ToHashSet();


            var moveHighlights = new MapHighlights(squares, HighlightLevel.PlayerMove);
            HighlightSignal.Dispatch(moveHighlights);

            var attackableSquares = squares.SelectMany(square => { return MathUtils.GetAdjacentPoints(square); })
                .Distinct()
                .Where(square => {
                    if (map.IsBlockedByEnvironment(square)) {
                        return false;
                    }

                    var occupant = map.GetAtPosition(square);
                    return occupant != null && occupant.Army.IsEnemyOf(Model.SelectedCombatant.Army);
                })
                .ToHashSet();

            var attackHighlights = new MapHighlights(attackableSquares, HighlightLevel.PlayerAttack);
            HighlightSignal.Dispatch(attackHighlights);
            HoverTileDisableSignal.Dispatch();
        }
    }
}
