using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ViewingUnitInfo : StateMachineBehaviour {
    private BattleState State;
    private Animator Animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = CombatObjects.GetBattleState();
        Animator = animator;

        UnitInfoManager.Instance.ShowUnitInfo(State.SelectedUnit);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Animator.SetTrigger("info_exit");
            State.ResetMovementState();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UnitInfoManager.Instance.HideUnitInfo();
    }
}