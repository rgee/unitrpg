using UnityEngine;

public class FriendlyUnitSelected : CancelableCombatState {
    private Animator _animator;
    private BattleState _battleState;
    private ActionMenuManager _menuManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _animator = animator;
        _battleState = GameObject.Find("BattleManager").GetComponent<BattleState>();
        _menuManager = CombatObjects.GetActionMenuManager();

        var model = _battleState.Model;
        var actions = model.GetAvailableActions(_battleState.SelectedUnit.model);

        _menuManager.OnActionSelected += HandleAction;
        _menuManager.ShowActionMenu(actions, _battleState.SelectedUnit);
    }

    private void HandleAction(BattleAction action) {
        switch (action) {
            case BattleAction.ATTACK:
                _animator.SetTrigger("fight_selected");
                break;
            case BattleAction.MOVE:
                _animator.SetTrigger("move_selected");
                break;
            case BattleAction.ITEM:
                _animator.SetTrigger("item_selected");
                break;
            case BattleAction.WAIT:
                _battleState.Model.WaitUnit(_battleState.SelectedUnit.model);
                _animator.SetTrigger("wait_selected");
                break;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _menuManager.OnActionSelected -= HandleAction;
        _menuManager.HideCurrentMenu();
    }
}