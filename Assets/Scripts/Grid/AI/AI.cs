using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AI : MonoBehaviour {

    public BattleManager BattleManager;
    public Grid.UnitManager UnitManager;
    public int AggroRadius = -1;
    
    private Seeker seeker;

    public void Awake() {
        seeker = GetComponent<Seeker>();
    }


    public void TakeTurn() {
        BattleManager.EndEnemyPhase();
    }
}
