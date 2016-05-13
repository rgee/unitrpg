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
        public UnitSelectedSignal UnitSelectedSignal { get; set; }

        [Inject]
        public NewMoveRangeSignal NewMoveRangeSignal { get; set; }

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
                    break;
                case BattleUIState.SelectingMoveLocation:
                    ClearHighlightSignal.Dispatch(HighlightLevel.PlayerMove);
                    PathUnavailableSignal.Dispatch();
                    Model.CurrentMovementPath = null;
                    break;
                case BattleUIState.CombatantMoving:
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
                case BattleUIState.SelectingFightAction:
                    break;
                case BattleUIState.SelectingAttackTarget:
                    break;
                case BattleUIState.SelectingMoveLocation:
                    var map = Model.Map;
                    var origin = Model.SelectedCombatant.Position;
                    var moveRange = Model.SelectedCombatant.GetAttribute(Attribute.AttributeType.Move);
                    var squares = map.BreadthFirstSearch(origin, moveRange.Value, false);

                    NewMoveRangeSignal.Dispatch(squares);
                    HoverTileDisableSignal.Dispatch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }
    }
}
