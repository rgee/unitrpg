using UnityEngine;

public class FightBegins : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetTrigger("fight_initialized");
    }
}