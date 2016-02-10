using System.Collections.Generic;

namespace Models.Fighting {
    public interface ICombatBuffProvider {
        List<IBuff> InitiatorPreCombatBuffs { get; }
        List<IBuff> InitiatorOnHitBuffs { get; } 

        List<IBuff> ReceiverPreCombatBuffs { get; } 
        List<IBuff> ReceiverOnHitBuffs { get; } 
    }
}