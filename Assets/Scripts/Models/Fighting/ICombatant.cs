using System.Collections.Generic;

namespace Models.Fighting {
    public interface ICombatant {
        int Health { get; }
        void TakeDamage(int amount);
        bool IsAlive { get; }
        Attribute GetAttribute(Attribute.AttributeType type);
        Stat GetStat(StatType type);
        List<IBuff> Buffs { get; }
        void AddBuff(IBuff buff);
        void RemoveBuff(string name);
        string PrimaryWeapon { get; set; }
        string SecondaryWeapon { get; set; }
        void AddTemporaryBuff(IBuff temporaryBuff);
        void RemoveTemporaryBuff(IBuff temporaryBuff);
    }
}