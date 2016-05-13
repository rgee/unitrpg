using System;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class StateTransitionCommand : Command {

        [Inject]
        public StateTransition Transition { get; set; }

        [Inject]
        public MovementPathUnavailableSignal PathUnavailableSignal { get; set; }

        [Inject]
        public HoverTileDisableSignal HoverTileDisableSignal { get; set; }

        public override void Execute() {
            Cleanup(Transition.Previous);
            Setup(Transition.Next);
        }

        private void Cleanup(BattleUIState state) {
            switch (state) {
                case BattleUIState.SelectingUnit:
                    break;
                case BattleUIState.SelectingAction:
                    break;
                case BattleUIState.SelectingFightAction:
                    break;
                case BattleUIState.SelectingAttackTarget:
                    break;
                case BattleUIState.SelectingMoveLocation:
                    PathUnavailableSignal.Dispatch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }

        private void Setup(BattleUIState state) {
            switch (state) {
                case BattleUIState.SelectingUnit:
                    break;
                case BattleUIState.SelectingAction:
                    break;
                case BattleUIState.SelectingFightAction:
                    break;
                case BattleUIState.SelectingAttackTarget:
                    break;
                case BattleUIState.SelectingMoveLocation:
                    HoverTileDisableSignal.Dispatch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }
    }
}
