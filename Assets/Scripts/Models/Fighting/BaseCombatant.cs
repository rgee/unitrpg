using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Buffs;
using Models.Fighting.Characters;
using Models.Fighting.Equip;
using UnityEngine;

namespace Models.Fighting {
    public class BaseCombatant : ICombatant {
        public int Health { get; protected set; }

        public Vector2 Position { get; set; }

        public bool IsAlive {
            get { return Health > 0; }
        }
        
        public List<IBuff> Buffs { get; private set; }
        
        public List<IBuff> _temporaryBuffs = new List<IBuff>();

        public HashSet<Weapon> EquippedWeapons { get; private set; }

        private readonly ICharacter _character;

        public BaseCombatant(ICharacter character) {
            Buffs = new List<IBuff>();
            _character = character;

            Health = character.Attributes.First(attr => attr.Type == Attribute.AttributeType.Health).Value;
            EquippedWeapons = character.Weapons
                .Select(name => WeaponDatabase.Instance.GetByName(name))
                .ToHashSet();
        }

        public void TakeDamage(int amount) {
            Health = Math.Max(Health - amount, 0);
        }

        public Attribute GetAttribute(Attribute.AttributeType type) {
            var baseAttr = _character.Attributes.First(attr => attr.Type == type);
            return AttributeUtils.ApplyBuffs(baseAttr, Buffs.Concat(_temporaryBuffs));
        }

        public Stat GetStat(StatType type) {
            var stat = _character.Stats.FirstOrDefault(_stat => _stat.Type == type);
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