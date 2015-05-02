using System.Collections;
using Grid;
using UnityEngine;

public class AIManager : MonoBehaviour {
    public float EnemyActionDelaySeconds = 0.2f;
    public UnitManager UnitManager;

    public IEnumerator TakeTurn() {
        foreach (var unit in UnitManager.GetEnemies()) {
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