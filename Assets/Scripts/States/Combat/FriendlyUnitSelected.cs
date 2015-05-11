using UnityEngine;

public class FriendlyUnitSelected : CancelableCombatState {
    private Animator Animator;
    private BattleState BattleState;
    private ActionMenuManager MenuManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Animator = animator;
        BattleState = GameObject.Find("BattleManager").GetComponent<BattleState>();
        MenuManager = GameObject.Find("ActionMenuManager").GetComponent<ActionMenuManager>();

        MenuManager.OnActionSelected += HandleAction;
        MenuManager.ShowActionMenu(BattleState.SelectedUnit);
    }

    private void HandleAction(BattleAction action) {
        switch (action) {
            case BattleAction.FIGHT:
                Animator.SetTrigger("fight_selected");
                break;
            case BattleAction.MOVE:
                Animator.SetTrigger("move_selected");
                break;
            case BattleAction.WAIT:
                BattleState.Model.WaitUnit(BattleState.SelectedUnit.model);
                Animator.SetTrigger("wait_selected");
                break;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        MenuManager.OnActionSelected -= HandleAction;
        MenuManager.HideCurrentMenu();
    }
}