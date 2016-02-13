using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class SkillResult {
        public readonly List<IEffect> InitiatorEffects;
        public readonly List<IEffect> ReceiverEffects;

        public SkillResult(List<IEffect> initiatorEffects, List<IEffect> receiverEffects) {
            InitiatorEffects = initiatorEffects;
            ReceiverEffects = receiverEffects;
        }

        public int GetAttackerDamage() {
            return InitiatorEffects.OfType<Damage>().Sum(dmgEffect => dmgEffect.Amount);
        }

        public int GetDefenderDamage() {
            return ReceiverEffects.OfType<Damage>().Sum(dmgEffect => dmgEffect.Amount);
        }
    }
}