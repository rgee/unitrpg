using System;
using System.Collections;
using Models.Combat;
using UI.ActionMenu;
using UnityEngine;

public class FriendlyUnitSelected : CancelableCombatState {
    private Animator _animator;
    private BattleState _battleState;
    private ActionMenu _menu;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _animator = animator;
        _battleState = GameObject.Find("BattleManager").GetComponent<BattleState>();
        _menu = CombatObjects.GetActionMenu();

        var model = _battleState.Model;
        var unitModel = _battleState.SelectedUnit.model;
        var actions = model.GetAvailableActions(unitModel);
        var fightActions = model.GetAvailableFightActions(unitModel);

        _menu.OnActionSelected += HandleAction;
        _menu.OnCancel += OnCancel;
        _menu.transform.position = _battleState.SelectedUnit.transform.position;
        _menu.Show(actions, fightActions);
    }

    protected override void OnCancel() {
        _menu.Hide();
        base.OnCancel(); 
    }
    
    private void HandleAction(CombatActionType actionType) {
        switch (actionType) {
            case CombatActionType.Move:
                _animator.SetTrigger("move_selected");
                break;
            case CombatActionType.Use:
                _animator.SetTrigger("use_selected");
                break;
            case CombatActionType.Fight:
                break;
            case CombatActionType.Item:
                _animator.SetTrigger("item_selected");
                break;
            case CombatActionType.Trade:
                break;
            case CombatActionType.Talk:
                break;
            case CombatActionType.Wait:
                _battleState.Model.WaitUnit(_battleState.SelectedUnit.model);
                _animator.SetTrigger("wait_selected");
                break;
            case CombatActionType.Brace:
                break;
            case CombatActionType.Cover:
                break;
            case CombatActionType.Attack:
                _animator.SetTrigger("attack_selected");
                break;
            default:
                throw new ArgumentException("Could not handle action " + actionType);
        }
        _menu.Hide();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _menu.OnActionSelected -= HandleAction;
    }
}