using System;
using System.Collections.Generic;
using Models.Combat.Inventory;
using UnityEngine;
using UnityEngine.UI;

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

        public void UseItem(Item item) {
            if (Inventory.Contains(item)) {
                TakeHeal(item.HealAmount);
                Inventory.Remove(item);
            }
        }

        private void TakeHeal(int heal) {
            Health = Math.Min(Character.MaxHealth, Health + heal);
        }

        public LevelUpResults LevelUp() {
            return Character.LevelUp();
        }
    }
}