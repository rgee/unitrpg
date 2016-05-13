using Models.Combat;

namespace Models.Fighting.Effects {
    public class Damage : IEffect {
        public int Amount { get; private set; }

        public Damage(int amount) {
            Amount = amount;
        }
        
        public void Apply(ICombatant combatant) {
           combatant.TakeDamage(Amount); 
        }
    }
}