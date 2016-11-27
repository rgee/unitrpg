using System.Collections.Generic;
using Models.Fighting.Battle;

namespace Models.Fighting.AI {
    public interface IEnemyAICoordinator {
        IList<ICombatAction> ComputeTurnActions();
    }
}