using UnityEngine;
using System.Collections;

namespace Models {
	public class Character : ScriptableObject {

		public int MaxHealth;
		public int Strength;
		public int Defense;
		public int Movement;
		public int Speed;

		public int SpeedGrowth;
		public int StrengthGrowth;
		public int DefenseGrowth;
		public int MovementGrowth;
		public bool IsEnemy;

		public string Name;
	}
}
