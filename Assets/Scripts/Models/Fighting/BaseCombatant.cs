using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Fighting {
    public abstract class BaseCombatant : ICombatant {
        public int Health { get; protected set; }

        public bool IsAlive {
            get { return Health > 0; }
        }

        public List<IBuff> Buffs { get; private set; }

        private Dictionary<StatType, IStat> _baseStats = new Dictionary<StatType, IStat>();

        private Dictionary<Attribute.AttributeType, Attribute> _attributes =
            new Dictionary<Attribute.AttributeType, Attribute>();

        protected BaseCombatant() {
            Buffs = new List<IBuff>();
        }

        public void TakeDamage(int amount) {
            Health = Math.Max(Health - amount, 0);
        }

        public Attribute GetAttribute(Attribute.AttributeType type) {
            return _attributes[type];
        }

        public IStat GetStat(StatType type) {
            return _baseStats[type];
        }

        public void AddBuff(IBuff buff) {
            Buffs.Add(buff); 
        }

        public void RemoveBuff(string name) {
            Buffs.RemoveAll(buff => buff.Name == name);
        }
    }
}