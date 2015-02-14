using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AI : MonoBehaviour {

    public BattleManager battleManager;
    public Grid.UnitManager unitManager;
    private Seeker seeker;

    public void Awake() {
        seeker = GetComponent<Seeker>();
    }


    public void TakeTurn() {
        battleManager.EndEnemyPhase();
    }
}
