using System.Collections.Generic;

namespace Models.Fighting.Buffs {
    public class SteelRapierGlanceChanceDebuff : AbstractBuff {
        public SteelRapierGlanceChanceDebuff() : base("steel_rapier_debuff") {
            StatMods = new Dictionary<StatType, StatMod>() {
                { StatType.GlanceChance, CreateMod(i => i + 10) }
            };
        }
    }
}