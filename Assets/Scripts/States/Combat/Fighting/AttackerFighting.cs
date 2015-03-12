using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AttackerFighting : StateMachineBehaviour {

    private Animator Animator;
    private BattleState State;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;

        State = CombatObjects.GetBattleState();

        Grid.Unit unit = State.SelectedUnit;
        unit.OnAttackComplete += OnAttackComplete;
        unit.Attack();
    }

    void OnAttackComplete() {
        State.AttackTarget.TakeDamage(1000);
        if (!State.AttackTarget.IsAlive()) {
            Animator.SetTrigger("defender_died");
        } else {
            Animator.SetTrigger("defender_survived");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State.SelectedUnit.OnAttackComplete -= OnAttackComplete;
    }
}
