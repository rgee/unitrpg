using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Kinesis : ISkillStrategy {
        public SkillResult Compute(Skill skill, IRandomizer randomizer) {
            var initiator = skill.Initiator;
            var myKinesis = initiator.GetAttribute(Attribute.AttributeType.Kinesis).Value;
            var postCombatBuffApplications = skill.ReceiverOnSuccessBuffs.Select(buff => new ApplyBuff(buff) as IEffect);
            var hit = new List<IEffect> {new Damage(myKinesis)};
            return new SkillResult(
                new List<IEffect>(), 
                hit.Concat(postCombatBuffApplications).ToList()
            );
        }
    }
}