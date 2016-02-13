using System.Collections.Generic;

namespace Models.Fighting {
    public class NullBuffProvider : ICombatBuffProvider {
        public List<IBuff> InitiatorPreCombatBuffs { get; private set; }
        public List<IBuff> ReceiverPreCombatBuffs { get; private set; }

        public NullBuffProvider() {
            InitiatorPreCombatBuffs = new List<IBuff>();
            ReceiverPreCombatBuffs = new List<IBuff>();
        }
    }
}