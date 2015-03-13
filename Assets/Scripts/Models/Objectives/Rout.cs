using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Rout : Objective {

    private Grid.UnitManager UnitManager;
    private Animator StateMachine;

    public void Awake() {
        UnitManager = CombatObjects.GetUnitManager();
        StateMachine = GetComponent<Animator>();
    }

    public void Update() {
        if (IsComplete()) {
            StateMachine.SetTrigger("battle_won");
        } else if (IsFailed()) {
            StateMachine.SetTrigger("battle_lost");
        }
    }

    public override bool IsComplete() {
        return !UnitManager.GetEnemies().Any();
    }

    public override bool IsFailed() {
        return !UnitManager.GetFriendlies().Any();
    }
}