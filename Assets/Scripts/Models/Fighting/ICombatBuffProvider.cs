using System.Collections.Generic;
using Models.Fighting.Buffs;

namespace Models.Fighting {
    public interface ICombatBuffProvider {
        List<IBuff> InitiatorPreCombatBuffs { get; }
        List<IBuff> ReceiverPreCombatBuffs { get; } 
    }
}