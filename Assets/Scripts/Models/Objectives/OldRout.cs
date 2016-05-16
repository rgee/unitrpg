using System.Linq;
using Grid;
using UnityEngine;

public class OldRout : Objective {
    private Animator StateMachine;
    private UnitManager UnitManager;

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