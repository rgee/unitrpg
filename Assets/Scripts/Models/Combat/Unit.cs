using Models;
using UnityEngine;

namespace Models.Combat {
	[System.Serializable]
	public class Unit {

		public Character Character;
		public Vector2 GridPosition;
	    public bool IsFriendly;
		public int Health;
		public bool IsAlive {
			get {
				return Health > 0;
			}
		}

		public void TakeDamage(int damage) {
			int resultingHealth = Health - (damage - Character.Defense);
			Health = Mathf.Max(0, resultingHealth);
		}

        public LevelUpResults LevelUp() {
            return Character.LevelUp();
        }
	}
}
