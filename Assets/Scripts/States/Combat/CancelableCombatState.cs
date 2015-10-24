using UnityEngine;

public class CancelableCombatState : StateMachineBehaviour {
    private BattleState State;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = GameObject.Find("BattleManager").GetComponent<BattleState>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            State.ResetToUnitSelectedState();
            animator.SetTrigger("action_canceled");
        }
    }
}