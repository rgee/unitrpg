using System;
using System.Collections.Generic;
using Models.Combat.Inventory;
using UnityEngine;

namespace Models.Combat {
    [Serializable]
    public class Unit {
        public Character Character;
        public Vector2 GridPosition;
        public int Health;
        public bool IsFriendly;
        public List<Item> Inventory; 

        public bool IsAlive {
            get { return Health > 0; }
        }

        public void TakeDamage(int damage) {
            var resultingHealth = Mathf.Max(0, Health - Mathf.Max(0, damage - Character.Defense));
            Health = resultingHealth;
        }

        public LevelUpResults LevelUp() {
            return Character.LevelUp();
        }
    }
}