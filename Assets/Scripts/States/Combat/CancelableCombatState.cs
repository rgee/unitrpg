using System;
using UnityEngine;

public class CancelableCombatState : StateMachineBehaviour {
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            animator.SetTrigger("action_canceled");
        }
    }
}