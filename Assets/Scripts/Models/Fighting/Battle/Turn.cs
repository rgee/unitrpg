﻿using System;
using System.Collections.Generic;

namespace Models.Fighting.Battle {
    public class Turn {
        private Dictionary<string, int> _movesRemaining = new Dictionary<string, int>();  
        private Dictionary<string, bool> _actionTaken = new Dictionary<string, bool>();

        public Turn(List<ICombatant> combatants) {
            combatants.ForEach(combatant => {
                _movesRemaining[combatant.Id] = combatant.GetAttribute(Attribute.AttributeType.Move).Value;
                _actionTaken[combatant.Id] = false;
            });
        }

        public void MarkMove(ICombatant combatant, int squares) {
            _movesRemaining[combatant.Id] = Math.Max(0, _movesRemaining[combatant.Id] - squares);
        }

        public void MarkAction(ICombatant combatant) {
            _actionTaken[combatant.Id] = true;
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