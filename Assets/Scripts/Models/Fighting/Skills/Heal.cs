using System.Collections.Generic;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Heal : AbstractSkillStrategy {
        protected override SkillResult Compute(ICombatant attacker, ICombatant defender, IRandomizer randomizer) { 
            var mySkill = attacker.GetAttribute(Attribute.AttributeType.Skill).Value;
            return new SkillResult(
                new List<IEffect>(), 
                new List<IEffect> {
                    new Damage(-mySkill)
                }
            );
        }
    }
}