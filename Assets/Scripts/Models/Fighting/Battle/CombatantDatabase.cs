using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Characters;

namespace Models.Fighting.Battle {
    public class CombatantDatabase : ICombatantDatabase {
        private readonly ILookup<ArmyType, ICombatant> _combatants; 

        public CombatantDatabase(List<ICombatant> combatants) {
            _combatants = combatants.ToLookup(c => c.Army, c => c);
        }

        public List<ICombatant> GetCombatantsByArmy(ArmyType army) {
            return _combatants[army].ToList();
        }
    }
}