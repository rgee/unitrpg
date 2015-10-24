using UnityEngine;

public class FightBegins : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var fightState = FightExecutionObjects.GetState();
        var hits = fightState.Result.InitialAttack.AttackerHits.Count;
        animator.SetBool("attacker_doubles", hits > 1);
        animator.SetTrigger("fight_initialized");
    }
}