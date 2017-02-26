using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Battle;
using UnityEngine;

namespace Models.Fighting.AI {
    public class AIActionPlan : IActionPlan {
        private readonly Stack<ICombatant> _combatants;

        public AIActionPlan(List<ICombatant> combatants) {
            combatants = combatants.Where(combatant => combatant.Brain != null)
                .ToList();
            _combatants = new Stack<ICombatant>(combatants);
        }

        public List<ICombatAction> NextActionStep(IBattle battle) {
            var combatant = _combatants.Pop();
            return combatant.Brain.ComputeActions(battle).ToList();
        }

        public bool HasActionsRemaining() {
            return _combatants.Count > 0;
        }
    }
}