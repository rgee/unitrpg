using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class SkllEffects {
        public readonly List<IEffect> ReceiverEffects;

        public int AttackCount {
            get { return ReceiverEffects.OfType<Damage>().Count(); }
        }

        public SkllEffects(List<IEffect> receiverEffects) {
            ReceiverEffects = receiverEffects;
        }

        public int GetDefenderDamage() {
            return ReceiverEffects.OfType<Damage>().Sum(dmgEffect => dmgEffect.Amount);
        }
    }
}