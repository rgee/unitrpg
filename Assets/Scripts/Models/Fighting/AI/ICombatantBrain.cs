using Models.Fighting.Battle;

namespace Models.Fighting.AI {
    /// <summary>
    /// AI Component that drives an individual combatant.
    /// </summary>
    public interface ICombatantBrain {
        ICombatAction ComputeAction(IBattle battle);
    }
}