using UnityEngine;

public class CancelableCombatState : StateMachineBehaviour {
    private BattleState _state;
    private Animator _animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _state = GameObject.Find("BattleManager").GetComponent<BattleState>();
        _animator = animator;
        CombatEventBus.Backs.AddListener(OnCancel);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OnCancel();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        CombatEventBus.Backs.RemoveListener(OnCancel);
    }

    protected virtual void OnCancel() {
        _state.ResetToUnitSelectedState();
        _animator.SetTrigger("action_canceled");
    }
}