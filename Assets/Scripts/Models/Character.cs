using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models {
    public class Character : ScriptableObject {
        public List<BattleAction> AvailableActions;
        public int Defense;
        public int DefenseGrowth;
        public int Exp;
        public int AttackRange;
        public bool IsEnemy;
        public int Level = 1;
        public int MaxHealth;
        public int Movement;
        public int MovementGrowth;
        public string Name;
        public int Skill;
        public int SkillGrowth;
        public int Speed;
        public int SpeedGrowth;
        public int Strength;
        public int StrengthGrowth;

        public void ApplyExp(int amount) {
            Exp = Math.Min(Exp + amount, 100);
        }

        public LevelUpResults LevelUp() {
            var result = new LevelUpResults();

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
            return Random.Range(0, 100) < growthPct;
        }
    }
}