using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Kinesis : AbstractSkillStrategy {
        protected override SkillResult Compute(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            var myKinesis = attacker.GetAttribute(Attribute.AttributeType.Kinesis).Value;
            var hit = new List<IEffect> {new Damage(myKinesis)};
            return new SkillResult(
                new List<IEffect>(), 
                hit
            );
        }
    }
}