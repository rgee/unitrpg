using System.Collections.Generic;
using Models.Fighting.Characters;

namespace Models.Fighting.Battle {
    public interface ICombatantDatabase {
        List<ICombatant> GetCombatantsByArmy(ArmyType army);
        List<ICombatant> GetAllCombatants();
    }
}