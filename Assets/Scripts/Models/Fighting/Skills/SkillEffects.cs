using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class SkillEffects {
        public readonly List<IEffect> ReceiverEffects;

        public int AttackCount {
            get { return ReceiverEffects.OfType<Damage>().Count(); }
        }

        public SkillEffects(List<IEffect> receiverEffects) {
            ReceiverEffects = receiverEffects;
        }

        public int GetDefenderDamage() {
            return ReceiverEffects.OfType<Damage>().Sum(dmgEffect => dmgEffect.Amount);
        }
    }
}