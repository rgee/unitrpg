using Models.Fighting.Effects;

namespace Models.Fighting.Buffs {
    public class SecondaryWeaponDebuff : ScaleDamageBuff {
        public SecondaryWeaponDebuff() : base(0.5f, "using_secondary_weapon") {
        }
    }
}