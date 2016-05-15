namespace Models.Fighting.Effects {
    public class WeaponCrit : WeaponHit {
        public WeaponCrit(int baseDamage) : base(2 * baseDamage) {
        }
    }
}