using System;
using UnityEngine;

public class ShowingEXP : StateMachineBehaviour {

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        BattleState state = CombatObjects.GetBattleState();
        state.MarkUnitActed(state.SelectedUnit);

        animator.SetTrigger("no_exp");
    }
}
