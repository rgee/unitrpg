using System;
using UnityEngine;

public class SelectingFightAction : StateMachineBehaviour {

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetTrigger("fight_action_selected");
    }
}