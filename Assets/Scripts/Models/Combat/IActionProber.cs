using System.Collections.Generic;

namespace Models.Combat {
    public interface IActionProber {
        IEnumerable<CombatAction> GetAvailableActions(Unit unit);
        IEnumerable<CombatAction> GetAvailableFightActions(Unit unit);
    }
}