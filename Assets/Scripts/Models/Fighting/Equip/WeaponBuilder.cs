using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Buffs;

namespace Models.Fighting.Equip {
    public class WeaponBuilder {
        private string _name;
        private string _description;
        private int _range;
        private List<IBuff> _initiatorPreBuffs = new List<IBuff>(); 
        private List<IBuff> _initiatorOnHitBuffs = new List<IBuff>();
        private List<IBuff> _receiverPreCombatBuffs = new List<IBuff>(); 
        private List<IBuff> _receiverOnHitBuffs = new List<IBuff>();

        public WeaponBuilder InitiatorPreBuffs(params IBuff[] buffs) {
            _initiatorPreBuffs = buffs.ToList();
            return this;
        }

        public WeaponBuilder InitiatorOnHitBuffs(params IBuff[] buffs) {
            _initiatorOnHitBuffs = buffs.ToList();
            return this;
        }

        public WeaponBuilder ReceiverPreBuffs(params IBuff[] buffs) {
            _receiverPreCombatBuffs = buffs.ToList();
            return this;
        }

        public WeaponBuilder ReceiverOnHitBuffs(params IBuff[] buffs) {
            _receiverOnHitBuffs = buffs.ToList();
            return this;
        }

        public WeaponBuilder IsSecondary() {
            _initiatorPreBuffs.Add(new SecondaryWeaponDebuff());
            return this;
        }

        public WeaponBuilder Name(string name) {
            _name = name;
            return this;
        }

        public WeaponBuilder Description(string description) {
            _description = description;
            return this;
        }

        public WeaponBuilder Range(int range) {
            _range = range;
            return this;
        }

        public Weapon Build() {
            return new Weapon(_name, _description, _range, _initiatorPreBuffs, _initiatorOnHitBuffs, _receiverPreCombatBuffs, _receiverOnHitBuffs);    
        }
    }
}