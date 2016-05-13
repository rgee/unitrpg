using System.Collections.Generic;

namespace Models.Fighting.Buffs {
    public class CompoundBowBuff : AbstractBuff {
        public CompoundBowBuff() : base("compound_bow_crit_buff") {
            StatMods = new Dictionary<StatType, StatMod>() {
                { StatType.CritChance, CreateMod(i => i + 15) }
            };
        }
    }
}