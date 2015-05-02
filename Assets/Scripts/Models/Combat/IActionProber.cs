using System.Collections.Generic;

namespace Models.Combat {
    public interface IActionProber {
        List<CombatAction> GetAvailableActions(Unit unit);
    }
}