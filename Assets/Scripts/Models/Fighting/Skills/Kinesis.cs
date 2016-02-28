using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Kinesis : AbstractSkillStrategy {
        public Kinesis() : base(SkillType.Kinesis, true, false) {
        }


        protected override SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender) {
            var attackCount = 1;
            var chances = new SkillChances {
                CritChance = 100,
                HitChance = 100,
                GlanceChance = 0
            };

            var myKinesis = attacker.GetAttribute(Attribute.AttributeType.Special).Value;
            var hit = new SkillHit {
                BaseDamage = myKinesis,
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
            return new NullBuffProvider();
        }

        protected override SkillEffects ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var myKinesis = attacker.GetAttribute(Attribute.AttributeType.Special).Value;
            var hit = new List<IEffect> {new Damage(myKinesis)};
            return new SkillEffects(hit);
        }
    }
}