using System.Collections.Generic;

namespace Models.Fighting.Skills {
    public struct Skill {
        /// <summary>
        /// The user of the skill
        /// </summary>
        public ICombatant Initiator;

        /// <summary>
        /// The combatant getting skilled on
        /// </summary>
        public ICombatant Receiver;

        /// <summary>
        /// Additional buffs to apply to the receiver before combat
        /// </summary>
        public List<IBuff> ReceiverPreBuffs;

        /// <summary>
        /// Persistent buffs to apply to the receiver after combat
        /// </summary>
        public List<IBuff> ReceiverOnSuccessBuffs;

        public Skill(ICombatant initiator, ICombatant receiver, List<IBuff> receiverPreBuffs, List<IBuff> receiverOnSuccessBuffs) {
            Initiator = initiator;
            Receiver = receiver;
            ReceiverPreBuffs = receiverPreBuffs;
            ReceiverOnSuccessBuffs = receiverOnSuccessBuffs;
        }
    }
}