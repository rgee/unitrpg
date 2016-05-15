namespace Models.Fighting.Effects {
    public class WeaponGlance : WeaponHit {
        public WeaponGlance(int baseDamage) : base(baseDamage / 2) {
        }
    }
}