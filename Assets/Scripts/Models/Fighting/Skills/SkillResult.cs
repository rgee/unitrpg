using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class SkillResult {
        public readonly List<IEffect> ReceiverEffects;

        public SkillResult(List<IEffect> receiverEffects) {
            ReceiverEffects = receiverEffects;
        }

        public int GetDefenderDamage() {
            return ReceiverEffects.OfType<Damage>().Sum(dmgEffect => dmgEffect.Amount);
        }
    }
}