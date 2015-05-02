using System;
using UnityEngine;

namespace Models.Combat {
    [Serializable]
    public class Unit {
        public Character Character;
        public Vector2 GridPosition;
        public int Health;
        public bool IsFriendly;

        public bool IsAlive {
            get { return Health > 0; }
        }

        public void TakeDamage(int damage) {
            var resultingHealth = Health - (damage - Character.Defense);
            Health = Mathf.Max(0, resultingHealth);
        }

        public LevelUpResults LevelUp() {
            return Character.LevelUp();
        }
    }
}