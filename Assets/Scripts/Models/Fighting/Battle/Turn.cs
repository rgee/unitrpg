using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Models.Fighting.Characters;

namespace Models.Fighting.Battle {
    public class Turn {
        private readonly Dictionary<string, int> _movesRemaining = new Dictionary<string, int>();  
        private readonly Dictionary<string, bool> _actionTaken = new Dictionary<string, bool>();
        private readonly List<ICombatant> _combatants;
        public readonly ArmyType Army;

        public Turn(ICombatantDatabase combatantDatabase, ArmyType army) {
            Army = army;
            _combatants = combatantDatabase.GetCombatantsByArmy(army);
            _combatants.ForEach(AddNewCombatant);
        }

        public void AddNewCombatant(ICombatant combatant) {
            _movesRemaining[combatant.Id] = combatant.GetAttribute(Attribute.AttributeType.Move).Value;
            _actionTaken[combatant.Id] = false;
        }

        public bool ShouldTurnEnd() {
            return !_combatants.Any(combatant => CanAct(combatant) || CanMove(combatant));
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