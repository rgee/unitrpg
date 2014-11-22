using UnityEngine;
using System.Collections;

namespace Models {
	public class Unit : MonoBehaviour {


		public Character Character;
		public Vector2 GridPosition;
		public int Health;
		private MapBehavior Map;

		void Awake() {
			Health = Character.MaxHealth;
			Map = GameObject.FindGameObjectWithTag("Map").GetComponent<MapBehavior>();
		}

		public void TakeDamage(int damage) {
			int resultingHealth = Health - (damage - Character.Defense);
			Health = Mathf.Max(0, resultingHealth);
		}
	}
}
