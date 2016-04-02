using UnityEngine;

namespace Models.Fighting.Effects {
    public class Advance : IEffect {
        private readonly ICombatant _attacker;

        public Advance(ICombatant attacker) {
            _attacker = attacker;
        }

        public void Apply(ICombatant combatant) {
            _attacker.MoveTo(combatant.Position);
        }
    }
}