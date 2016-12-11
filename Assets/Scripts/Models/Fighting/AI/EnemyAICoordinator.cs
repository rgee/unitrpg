using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Battle;
using Models.Fighting.Characters;

namespace Models.Fighting.AI {
    /// <summary>
    /// Coordinates AI across all enemies.
    /// </summary>
    public class EnemyAICoordinator : IEnemyAICoordinator {
        private readonly IBattle _battle;

        public EnemyAICoordinator(IBattle battle) {
            _battle = battle;
        }

        public IList<ICombatAction> ComputeTurnActions() {
            var livingEnemies = _battle.GetAliveByArmy(ArmyType.Enemy);
            var brains = livingEnemies.Select(enemy => enemy.Brain).Where(brain => brain != null).ToList();

            return brains.Select(brain => brain.ComputeAction(_battle)).ToList();
        }
    }
}