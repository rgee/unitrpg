using Models.Fighting.Effects;

namespace Models.Fighting.Buffs {
    public class StealthDamageReduction : ScaleDamageBuff {
        public StealthDamageReduction() : base(0.5f, "stealth_damage_reduction") { }
    }
}