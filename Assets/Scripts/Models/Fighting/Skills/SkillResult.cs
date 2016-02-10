using System.Collections.Generic;

namespace Models.Fighting.Skills {
    public struct SkillResult {
        public readonly List<IEffect> InitiatorEffects;
        public readonly List<IEffect> ReceiverEffects;

        public SkillResult(List<IEffect> initiatorEffects, List<IEffect> receiverEffects) {
            InitiatorEffects = initiatorEffects;
            ReceiverEffects = receiverEffects;
        }
    }
}