using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Combat {
    public class Turn : ITurn {
        private readonly IMap _map;
        private IDictionary<Unit, UnitTurnState> _turnState;
        private bool _friendlyComplete;

        public Turn(IMap map) {
            _map = map;
            InitializeTurn();
        }

        public int TurnCount { get; private set; }
        public TurnControl Control { get; private set; }

        public void End(TurnControl control) {
            if (_friendlyComplete) {
                if (control == TurnControl.Enemy) {
                    TurnCount++;
                    _friendlyComplete = false;
                }
            } else if (control == TurnControl.Friendly) {
                _friendlyComplete = true;
            }

            Control = control == TurnControl.Friendly ? TurnControl.Enemy : TurnControl.Friendly;
            InitializeTurn();
        }

        public void RecordMove(Unit unit, int squares) {
            var state = _turnState[unit];
            var moves = state.MovesRemaining;
            state.MovesRemaining -= squares;

            Debug.Log("Moves remaining reduced from " + moves + " to " + state.MovesRemaining);
        }

        public void RecordAction(Unit unit) {
            var state = _turnState[unit];
            state.Acted = true;
        }

        public int GetUsedMoves(Unit unit) {
            var total = unit.Character.Movement;
            var remaining = GetRemainingMoves(unit);

            return total - remaining;
        }

        public int GetRemainingMoves(Unit unit) {
            return _turnState[unit].MovesRemaining;
        }

        public bool CanAct(Unit unit) {
            return !_turnState[unit].Acted;
        }

        private void InitializeTurn() {
            _turnState = new Dictionary<Unit, UnitTurnState>();
            foreach (var unit in GetCurrentTurnUnits()) {
                _turnState[unit] = CreateState(unit);
            }
        }

        private IEnumerable<Unit> GetCurrentTurnUnits() {
            if (Control == TurnControl.Enemy) {
                return _map.GetEnemyUnits();
            }

            return _map.GetFriendlyUnits();
        }

        private static UnitTurnState CreateState(Unit unit) {
            return new UnitTurnState(unit.Character.Movement, false);
        }

        private class UnitTurnState {
            public bool Acted;
            public int MovesRemaining;

            public UnitTurnState(int movesRemaining, bool acted) {
                MovesRemaining = movesRemaining;
                Acted = acted;
            }
        }
    }
}