using System.Collections.Generic;

namespace Models.Fighting.Equip {
    public class Weapon : ICombatBuffProvider {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Range { get; private set; }
        public List<IBuff> InitiatorPreCombatBuffs { get; private set; }
        public List<IBuff> ReceiverPreCombatBuffs { get; private set; }

        public Weapon(string name, string description, int range, List<IBuff> initiatorPreCombatBuffs, 
            List<IBuff> receiverPreCombatBuffs) {

            Name = name;
            Description = description;
            Range = range;
            InitiatorPreCombatBuffs = initiatorPreCombatBuffs;
            ReceiverPreCombatBuffs = receiverPreCombatBuffs;
        }

        public static WeaponBuilder Builder() {
            return new WeaponBuilder();
        }
    }
}