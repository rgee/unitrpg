using UnityEngine;
using System.Collections;

namespace Models {
	public class Unit {

		public Character Character;
		public Vector2 GridPosition;
		public int Health;

		void Awake() {
			Health = Character.MaxHealth;
		}

		public void TakeDamage(int damage) {
			int resultingHealth = Health - (damage - Character.Defense);
			Health = Mathf.Max(0, resultingHealth);
		}
	}
}
