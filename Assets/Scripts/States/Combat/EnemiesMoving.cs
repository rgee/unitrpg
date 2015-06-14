using System.Collections;
using Models.Combat;
using UnityEngine;

public class EnemiesMoving : StateMachineBehaviour {
    private Animator Animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;
        AIManager.Instance.StartCoroutine(RunAI());
    }

    private IEnumerator RunAI() {
        yield return AIManager.Instance.StartCoroutine(AIManager.Instance.TakeTurn());
        Animator.SetTrigger("enemies_acted");
        CombatObjects.GetBattleState().Model.EndTurn(TurnControl.Enemy);
    }
}