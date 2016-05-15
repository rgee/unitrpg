using System;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using strange.extensions.command.impl;
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

        public override void Execute() {
            Cleanup(Transition.Previous);
            Setup(Transition.Next);
        }

        private void Cleanup(BattleUIState state) {
            switch (state) {
                case BattleUIState.SelectingUnit:
                    break;
                case BattleUIState.SelectingAction:
                    UnitDeselectedSignal.Dispatch();
                    break;
                case BattleUIState.SelectingFightAction:
                    break;
                case BattleUIState.SelectingAttackTarget:
                    ClearHighlightSignal.Dispatch(HighlightLevel.PlayerAttack);
                    break;
                case BattleUIState.SelectingMoveLocation:
                    ClearHighlightSignal.Dispatch(HighlightLevel.PlayerMove);
                    PathUnavailableSignal.Dispatch();
                    Model.CurrentMovementPath = null;
                    break;
                case BattleUIState.Fighting:
                    break;
                case BattleUIState.CombatantMoving:
                    break;
                case BattleUIState.ForecastingCombat:
                    FightForecastDisableSignal.Dispatch();
                    break;
                case BattleUIState.Uninitialized:
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
                    break;
                case BattleUIState.SelectingAction:
                    break;
                case BattleUIState.ForecastingCombat:
                    break;
                case BattleUIState.SelectingFightAction:
                    break;
                case BattleUIState.SelectingAttackTarget:
                    SetupAttackTargetState();
                    break;
                case BattleUIState.SelectingMoveLocation:
                    SetupMoveLocationState();
                    break;
                case BattleUIState.Fighting:
                    break;
                case BattleUIState.Uninitialized:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        private void SetupAttackTargetState() {
            var map = Model.Map;
            var battle = Model.Battle;
            var origin = Model.SelectedCombatant.Position;
            var range = battle.GetMaxWeaponAttackRange(Model.SelectedCombatant);
            var attackableSquares = map.BreadthFirstSearch(origin, range, true);
            var highlights = new MapHighlights(attackableSquares, HighlightLevel.PlayerAttack);

            HighlightSignal.Dispatch(highlights);
            HoverTileDisableSignal.Dispatch();
        }

        private void SetupMoveLocationState() {
            var map = Model.Map;
            var origin = Model.SelectedCombatant.Position;
            var moveRange = Model.Battle.GetRemainingMoves(Model.SelectedCombatant);
            var squares = map.BreadthFirstSearch(origin, moveRange, false);
            var highlights = new MapHighlights(squares, HighlightLevel.PlayerMove);

            HighlightSignal.Dispatch(highlights);
            HoverTileDisableSignal.Dispatch();
        }
    }
}
