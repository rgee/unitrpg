using UnityEngine;

public class ViewingInventory : StateMachineBehaviour {
    private InventoryPopupManager _popupManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _popupManager = CombatObjects.GetInventoryPopupManager();

        var unit = CombatObjects.GetBattleState().SelectedUnit;
        _popupManager.ShowInventory(unit);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            animator.SetTrigger("action_canceled");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _popupManager.HideInventory();
    }
}
