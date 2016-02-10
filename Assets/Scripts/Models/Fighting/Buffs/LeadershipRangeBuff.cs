using System.Collections.Generic;

namespace Models.Fighting.Buffs {
    public class LeadershipRangeBuff : AbstractBuff {
        public LeadershipRangeBuff(int amount, string name) : base(name) {
            StatMods = new Dictionary<StatType, StatMod>() {
                { StatType.LeadershipRange, CreateMod(i => i + amount) }
            };
        }
    }
}