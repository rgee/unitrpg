using UnityEngine;
using System.Collections;

namespace Models {
	[System.Serializable]
	public class Unit {

		public Character Character;
		public Vector2 GridPosition;
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
	}
}
