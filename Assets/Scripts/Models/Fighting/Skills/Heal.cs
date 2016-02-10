using System.Collections.Generic;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Heal : ISkillStrategy {
        public SkillResult Compute(Skill skill, IRandomizer randomizer) { 
            var initiator = skill.Initiator;
            var mySkill = initiator.GetAttribute(Attribute.AttributeType.Skill).Value;
            return new SkillResult(
                new List<IEffect>(), 
                new List<IEffect> {
                    new Damage(-mySkill)
                }
            );
        }
    }
}