using System;
using System.Collections.Generic;
using Models.Fighting.Buffs;
using Models.Fighting.Characters;
using Models.Fighting.Equip;
using UnityEngine;

namespace Models.Fighting {
    public interface ICombatant {
        string Id { get; }
        int Health { get; }
        void TakeDamage(int amount);
        bool IsAlive { get; }
        string Name { get; }
        Vector2 Position { get; set; }
        Attribute GetAttribute(Attribute.AttributeType type);
        Stat GetStat(StatType type);
        List<IBuff> Buffs { get; }
        void AddBuff(IBuff buff);
        void RemoveBuff(string name);
        void MoveTo(Vector2 destination);
        HashSet<Weapon> EquippedWeapons { get; } 
        void AddTemporaryBuff(IBuff temporaryBuff);
        void RemoveTemporaryBuff(IBuff temporaryBuff);
        ArmyType Army { get; set; }
    }
}