using System;
using System.Collections.Generic;
using Models.Combat;
using Models.Fighting.Skills;

namespace Models.Fighting.Buffs {
    public abstract class AbstractBuff : IBuff {
        public Unit Host { get; set; }
        public string Name { get; private set; }

        public IDictionary<StatType, StatMod> StatMods { get; protected set; }

        protected AbstractBuff(string name) {
            StatMods = new Dictionary<StatType, StatMod>();
            Name = name;
        }

        protected StatMod CreateMod(Func<int, int> modifierFuntion) {
            return new StatMod(Name, modifierFuntion);            
        }

        public virtual bool AppliesToSkill(SkillType type) {
            return true;
        }

        public virtual bool CanApply(IBattle battle) {
            return true;
        }
        
        public virtual Stat Modify(Stat stat) {
            var mod = StatMods[stat.Type];
            if (mod != null) {
               var newValue = mod.Func(stat.Value);
               return new Stat(newValue, stat.Type);
            } 
            
            return stat;
        }
        
        public virtual IEffect Modify(IEffect effect) {
            return effect;
        }

        public virtual Attribute Apply(Attribute attribute) {
            return attribute;
        }
    }
}