using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Heal : AbstractSkillStrategy {
        public Heal() : base(SkillType.Heal, false, false) {
        }

        protected override SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender) {
            var attackCount = 1;
            var chances = new SkillChances {
                CritChance = 100,
                HitChance = 100,
                GlanceChance = 0
            };

            var mySkill = attacker.GetAttribute(Attribute.AttributeType.Skill).Value;
            var hit = new SkillHit {
                BaseDamage = -mySkill,
                HitCount = attackCount
            };

            return new SkillForecast {
                Type = Type,
                Hit = hit,
                Chances = chances,
                Attacker = attacker,
                Defender = defender
            };
        }

        protected override ICombatBuffProvider GetBuffProvider(ICombatant attacker) {
            return attacker.EquippedWeapons.First(weapon => weapon.Range == 1);
        }

        protected override SkillEffects ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) { 
            var mySkill = attacker.GetAttribute(Attribute.AttributeType.Skill).Value;
            return new SkillEffects(
                new List<IEffect> {
                    new Damage(-mySkill)
                }
            );
        }
    }
}