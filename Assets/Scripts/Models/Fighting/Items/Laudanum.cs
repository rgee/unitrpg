using Models.Fighting.Effects;

namespace Models.Fighting.Items {
    public class Laudanum : BaseItem {
        public Laudanum() : base("Laudanum", "laudanum") {
        }

        public override void Use(ICombatant combatant) {
            var heal = new Damage(-10);
            heal.Apply(combatant);
        }
    }
}