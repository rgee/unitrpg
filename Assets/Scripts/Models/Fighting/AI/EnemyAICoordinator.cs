using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using UnityEngine;

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
            var enemiesWithAi = livingEnemies.Where(enemy => enemy.Brain != null).ToList();

            return enemiesWithAi.Select(enemy => {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = enemy.Brain.ComputeAction(_battle);
                var endTime = watch.ElapsedMilliseconds;

                Debug.LogFormat("Enemy {0} took {1} milliseconds to compute move", enemy.Name, endTime);
                return result;
            })
            .Where(action => action != null)
            .ToList();
        }
    }
}