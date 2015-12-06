using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Models.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models {
    public class Character : ScriptableObject, ISerializable {
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
        public float MoveTimePerSquare = 0.3f;
        public List<CombatAction> Actions; 

        public Character(Character character) {
            Defense = character.Defense;
            DefenseGrowth = character.DefenseGrowth;
            Exp = character.Exp;
            AttackRange = character.AttackRange;
            IsEnemy = character.IsEnemy;
            Level = character.Level;
            MaxHealth = character.MaxHealth;
            Movement = character.Movement;
            MovementGrowth = character.MovementGrowth;
            Name = character.Name;
            Skill = character.Skill;
            SkillGrowth = character.SkillGrowth;
            Speed = character.Speed;
            SpeedGrowth = character.SpeedGrowth;
            Strength = character.Strength;
            StrengthGrowth = character.StrengthGrowth;
        }

        protected Character(SerializationInfo info, StreamingContext context) {
            Name = info.GetString("name");
            Level = info.GetInt32("level");
            Exp = info.GetInt32("exp");
            MaxHealth = info.GetInt32("maxHealth");
            Defense = info.GetInt32("defense");
            Movement = info.GetInt32("movement");
            Skill = info.GetInt32("skill");
            Speed = info.GetInt32("speed");
            Strength = info.GetInt32("strength");
        }

        public Character() {
        }

        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("name", Name);
            info.AddValue("level", Name);
            info.AddValue("exp", Exp);
            info.AddValue("maxHealth", MaxHealth);
            info.AddValue("defense", Defense);
            info.AddValue("movement", Movement);
            info.AddValue("skill", Skill);
            info.AddValue("speed", Speed);
            info.AddValue("strength", Strength);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            GetObjectData(info, context);
        }

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