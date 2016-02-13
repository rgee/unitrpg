using System.Collections.Generic;

namespace Models.Fighting {
    public interface ICombatBuffProvider {
        List<IBuff> InitiatorPreCombatBuffs { get; }
        List<IBuff> ReceiverPreCombatBuffs { get; } 
    }
}