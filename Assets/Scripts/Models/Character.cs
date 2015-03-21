using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Models {
	public class Character : ScriptableObject {

        public int Level = 1;
		public int Exp = 0;

		public int MaxHealth;
		public int Strength;
		public int Defense;
		public int Movement;
		public int Speed;
		public int Skill;

		public int SpeedGrowth;
		public int StrengthGrowth;
		public int DefenseGrowth;
		public int MovementGrowth;
		public int SkillGrowth;
		public bool IsEnemy;

		public string Name;

		public List<BattleAction> AvailableActions;

		public void ApplyExp(int amount) {
			Exp = Math.Min(Exp+amount, 100);
		}

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

			if (DidLevel(SkillGrowth)) {
				Skill++;
				result.Skill = true;
			}

            return result;
        }

        private bool DidLevel(int growthPct) {
            return UnityEngine.Random.Range(0, 100) < growthPct;
        }
	}
}
