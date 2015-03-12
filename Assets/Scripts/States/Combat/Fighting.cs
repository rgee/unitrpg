using System;
using UnityEngine;

public class Fighting : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        BattleState state = GameObject.Find("BattleManager").GetComponent<BattleState>();

        state.MarkUnitActed(state.SelectedUnit);
        animator.SetTrigger("fight_completed");
    }
}