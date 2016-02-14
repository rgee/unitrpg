using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Buffs;
using Models.Fighting.Skills;
using UnityEngine;

namespace Models.Fighting {
    public abstract class BaseCombatant : ICombatant {
        public int Health { get; protected set; }

        public Vector2 Position { get; set; }

        public bool IsAlive {
            get { return Health > 0; }
        }
        
        public string PrimaryWeapon { get; set; }
        
        public string SecondaryWeapon { get; set; }
        
        public List<IBuff> Buffs { get; private set; }
        
        public List<IBuff> _temporaryBuffs = new List<IBuff>();

        private Dictionary<StatType, Stat> _baseStats = new Dictionary<StatType, Stat>();

        private Dictionary<int, ISkillStrategy> _strategies = new Dictionary<int, ISkillStrategy>(); 

        private Dictionary<Attribute.AttributeType, Attribute> _attributes =
            new Dictionary<Attribute.AttributeType, Attribute>();

        public ISkillStrategy GetStrategyByDistance(int distance) {
            return _strategies[distance];
        }

        protected BaseCombatant() {
            Buffs = new List<IBuff>();
        }

        public void TakeDamage(int amount) {
            Health = Math.Max(Health - amount, 0);
        }

        public Attribute GetAttribute(Attribute.AttributeType type) {
            var baseAttr = _attributes[type];
            return AttributeUtils.ApplyBuffs(baseAttr, Buffs.Concat(_temporaryBuffs));
        }

        public Stat GetStat(StatType type) {
            var stat = _baseStats[type];
            return StatUtils.ApplyBuffs(stat, Buffs.Concat(_temporaryBuffs));
        }

        public void AddBuff(IBuff buff) {
            Buffs.Add(buff); 
        }

        public void RemoveBuff(string name) {
            Buffs.RemoveAll(buff => buff.Name == name);
        }
        
        public void AddTemporaryBuff(IBuff buff) {
           _temporaryBuffs.Add(buff); 
        }
        
        public void RemoveTemporaryBuff(IBuff buff) {
           _temporaryBuffs.RemoveAll((b) => b.Name == buff.Name); 
        }
    }
}