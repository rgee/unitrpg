using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AI : MonoBehaviour {

    public BattleManager BattleManager;
    public Grid.UnitManager UnitManager;
    public float EnemyActionDelaySeconds = 0.2f;
    
    private Seeker seeker;

    public void Awake() {
        seeker = GetComponent<Seeker>();
    }


    public IEnumerator TakeTurn() {
        foreach (Grid.Unit unit in UnitManager.GetEnemies()) {

            // Unfortunately Unity requires us to use the non-generic method to get a component
            // that is an interface type.
            AIStrategy strat = (AIStrategy) unit.GetComponent(typeof(AIStrategy));
            if (strat != null) {
                yield return StartCoroutine(strat.act());
            } else {
                yield return null;
            }

            // Add in some delay between each unit taking its turn.
            yield return new WaitForSeconds(EnemyActionDelaySeconds);
        }

        BattleManager.EndEnemyPhase();
    }
}
