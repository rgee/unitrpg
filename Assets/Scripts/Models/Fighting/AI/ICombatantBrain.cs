using System.Collections.Generic;
using Models.Fighting.Battle;

namespace Models.Fighting.AI {
    /// <summary>
    /// AI Component that drives an individual combatant.
    /// </summary>
    public interface ICombatantBrain {
        IEnumerable<ICombatAction> ComputeActions(IBattle battle);
    }
}