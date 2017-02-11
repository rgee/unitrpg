using System.Collections.Generic;
using JetBrains.Annotations;
using Models.Fighting.Battle;

namespace Models.Fighting.AI {
    public interface IActionPlan {
        [CanBeNull]
        List<ICombatAction> NextActionStep(IBattle battle);

        bool HasActionsRemaining();
    }
}