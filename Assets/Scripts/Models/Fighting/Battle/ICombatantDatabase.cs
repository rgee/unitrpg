using System.Collections.Generic;

namespace Models.Fighting.Battle {
    public interface ICombatantDatabase {
        List<ICombatant> GetCombatantsByArmy(ArmyType army);
    }
}