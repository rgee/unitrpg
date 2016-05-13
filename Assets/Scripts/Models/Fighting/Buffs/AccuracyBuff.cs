using System.Collections.Generic;

namespace Models.Fighting.Buffs {
    public class AccuracyBuff : AbstractBuff {
        public AccuracyBuff(int amount, string name) : base(name) {
            StatMods = new Dictionary<StatType, StatMod>() {
                { StatType.HitChance, CreateMod(i => i + amount) }
            };
        }
    }
}