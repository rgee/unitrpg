using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Models {
	public class Character : ScriptableObject {

        public int level = 1;

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

		public List<BattleAction> AvailableActions;

        public LevelUpResults LevelUp() {
            LevelUpResults result = new LevelUpResults();

            if (DidLevel(SpeedGrowth)) {
                Speed++;
                result.Speed = true;
            }

            if (DidLevel(StrengthGrowth)) {
                Strength++;
                result.Strength = true;
            }


            if (DidLevel(Defense)) {
                Defense++;
                result.Defense = true;
            }

            if (DidLevel(MovementGrowth)) {
                Movement++;
                result.Movement = true;
            }
            return result;
        }

        private bool DidLevel(int growthPct) {
            return Random.Range(0, 100) < growthPct;
        }
	}
}
