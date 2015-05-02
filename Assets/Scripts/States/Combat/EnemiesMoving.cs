using System.Collections;
using UnityEngine;

public class EnemiesMoving : StateMachineBehaviour {
    private AIManager AI;
    private Animator Animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AI = CombatObjects.GetAIManager();
        Animator = animator;

        AI.StartCoroutine(RunAI());
    }

    private IEnumerator RunAI() {
        yield return AI.StartCoroutine(AI.TakeTurn());
        Animator.SetTrigger("enemies_acted");
    }
}