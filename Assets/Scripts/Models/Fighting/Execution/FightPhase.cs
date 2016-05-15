using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting.Execution {
    /// <summary>
    /// The effects of a single phase of a fight. 
    /// e.g. in a fight where there is one attack, and one counterattack, there are two phases.
    /// </summary>
    public class FightPhase {
        /// <summary>
        /// The combatant who acts during this phase.
        /// </summary>
        public ICombatant Initiator { get; set; }

        /// <summary>
        /// The combatant who receives the action.
        /// </summary>
        public ICombatant Receiver { get; set; }

        /// <summary>
        /// The receiver's response to the action in this phase.
        /// </summary>
        public DefenderResponse Response { get; set; }

        /// <summary>
        /// The thing(s) that happen(s) to the receiver after this fight phase.
        /// </summary>
        public SkillEffects Effects { get; set; }

        /// <summary>
        /// Whether or not the receiver of this fight phase dies from it.
        /// </summary>
        public bool ReceverDies { get; set; }

        /// <summary>
        /// The skill that was used by the initiator.
        /// </summary>
        public SkillType Skill { get; set; }
    }
}