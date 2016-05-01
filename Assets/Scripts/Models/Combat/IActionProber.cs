using System.Collections.Generic;

namespace Models.Combat {
    public interface IActionProber {
        IEnumerable<CombatActionType> GetAvailableActions(Unit unit);
        IEnumerable<CombatActionType> GetAvailableFightActions(Unit unit);
    }
}