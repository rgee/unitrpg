using Models.Combat;
using Models.Combat.Inventory;
using UnityEngine;

public class ViewingInventory : StateMachineBehaviour {
    private InventoryPopupManager _popupManager;
    private InventoryPopup _popup;
    private IBattle _battle;
    private Animator _animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _popupManager = CombatObjects.GetInventoryPopupManager();
        _battle = CombatObjects.GetBattleState().Model;

        var unit = CombatObjects.GetBattleState().SelectedUnit;
        _popup = _popupManager.ShowInventory(unit);
        _popup.OnItemUse += UseItem;
        _animator = animator;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            animator.SetTrigger("action_canceled");
        }
    }

    private void UseItem(Item item) {
        CombatObjects.GetBattleState().SelectedItem = item;
        _animator.SetTrigger("heal_item_selected");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _popup.OnItemUse -= UseItem;
        _popupManager.HideInventory();
    }
}
