using System.Collections;
using Grid;
using UnityEngine;

public class AIManager : Singleton<AIManager> {
    public float EnemyActionDelaySeconds = 0.2f;

    public IEnumerator TakeTurn() {
        var unitManager = CombatObjects.GetUnitManager();
        foreach (var unit in unitManager.GetEnemies()) {
            // Unfortunately Unity requires us to use the non-generic method to get a component
            // that is an interface type.
            var strat = (AIStrategy) unit.GetComponent(typeof (AIStrategy));
            if (strat != null) {
                yield return StartCoroutine(strat.act());
            } else {
                yield return null;
            }

            // Add in some delay between each unit taking its turn.
            yield return new WaitForSeconds(EnemyActionDelaySeconds);
        }
    }
}