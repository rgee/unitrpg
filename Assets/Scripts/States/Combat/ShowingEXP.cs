using System;
using UnityEngine;

public class ShowingEXP : StateMachineBehaviour {

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetTrigger("no_exp");
    }
}
