using Models.Combat;
using UI.ActionMenu;
using UnityEngine;

public class SelectingFightAction : CancelableCombatState {
    private BattleState _context;
    private ActionMenu _menu;
    private Animator _animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _menu = CombatObjects.GetActionMenu();
        _context = CombatObjects.GetBattleState();
        _animator = animator;

        var model = _context.Model;
        var actions = model.GetAvailableFightActions(_context.SelectedUnit.model);
        _menu.OnActionSelected += HandleAction;

        _menu.transform.position = _context.SelectedUnit.transform.position;
        _menu.Show(actions);
    }

    private void HandleAction(CombatAction action) {
        if (action == CombatAction.Attack) {
            _animator.SetTrigger("fight_action_selected");
        }

        // TODO: Add brace and cover
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _menu.OnActionSelected -= HandleAction;
    }
}