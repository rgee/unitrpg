﻿using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.AI;
using Models.Fighting.AI.Brains;
using Models.Fighting.Buffs;
using Models.Fighting.Characters;
using Models.Fighting.Equip;
using Models.Fighting.Skills;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Models.Fighting {
    public class BaseCombatant : ICombatant {
        public int Health { get; protected set; }

        public Vector2 Position { get; set; }

        public string Id { get; set; }

        public ICombatantBrain Brain { get; set; }

        public bool IsAlive {
            get { return Health > 0; }
        }

        public string Name { get; set; }
        
        public List<IBuff> Buffs { get; private set; }

        private readonly List<IBuff> _temporaryBuffs = new List<IBuff>();

        public HashSet<Weapon> EquippedWeapons { get; private set; }

        public ArmyType Army { get; set; }

        public SkillType? SpecialSkill { get; set; }

        public Signal<Vector2> MoveSignal { get; private set; }

        public Signal DeathSignal { get; private set; }

        private readonly ICharacter _character;

        public BaseCombatant(ICharacter character, ArmyType army) {
            Buffs = new List<IBuff>();
            _character = character;
            Army = army;
            Name = character.Name;
            MoveSignal = new Signal<Vector2>();
            DeathSignal = new Signal();
            SpecialSkill = character.SpecialSkill;

            Health = character.Attributes.First(attr => attr.Type == Attribute.AttributeType.Health).Value;
            EquippedWeapons = character.Weapons
                .Select(name => WeaponDatabase.Instance.GetByName(name))
                .ToHashSet();

            if (army == ArmyType.Enemy) {
                Brain = new Guard(this);
            }
        }

        public void TakeDamage(int amount) {
            Health = Math.Max(Health - amount, 0);
            if (Health == 0) {
                DeathSignal.Dispatch();
            }
        }

        public void MoveTo(Vector2 destination) {
            MoveSignal.Dispatch(destination);
        }

        public Attribute GetAttribute(Attribute.AttributeType type) {
            var baseAttr = _character.Attributes.First(attr => attr.Type == type);
            return AttributeUtils.ApplyBuffs(baseAttr, Buffs.Concat(_temporaryBuffs));
        }

        public Stat GetStat(StatType type) {
            var stat = _character.Stats.FirstOrDefault(potentialStat => potentialStat.Type == type);
            if (stat == null) {
                stat = new Stat(0, StatType.ProjectileParryChance);
            }

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