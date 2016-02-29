using System;
using UnityEngine;

namespace Models.Fighting.Execution {
    public class DamageTallyer {
        private readonly int _startingHealth;
        private int _accumulatedDamage;

        public DamageTallyer(ICombatant combatant) {
            _startingHealth = combatant.Health;
        }

        public bool IsDead() {
            return _accumulatedDamage >= _startingHealth;
        }

        public void ApplyDamage(int damage) {
            _accumulatedDamage = (int)Mathf.Clamp(_accumulatedDamage + damage, 0, _startingHealth);
        }
    }
}