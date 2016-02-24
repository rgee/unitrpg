using System;
using System.Collections.Generic;

namespace Models.Fighting.Battle {
    public class Turn {
        private Dictionary<string, int> _movesRemaining = new Dictionary<string, int>();  
        private Dictionary<string, bool> _actionTaken = new Dictionary<string, bool>();

        public void MarkMove(ICombatant combatant, int squares) {
            _movesRemaining[combatant.Id] = Math.Max(0, _movesRemaining[combatant.Id] - squares);
        }

        public void MarkAction(ICombatant combatant) {
            _actionTaken[combatant.Id] = false;
        }

        public int GetRemainingMoveDistance(ICombatant combatant) {
            return _movesRemaining[combatant.Id];
        }

        public bool CanAct(ICombatant combatant) {
            return !_actionTaken[combatant.Id];
        }

        public bool CanMove(ICombatant combatant) {
            return GetRemainingMoveDistance(combatant) > 0;
        }
    }
}