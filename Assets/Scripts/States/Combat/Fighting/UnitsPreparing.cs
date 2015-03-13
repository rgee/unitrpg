using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class UnitsPreparing : StateMachineBehaviour {
    private BattleState State;
    private Animator Animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;
        State = CombatObjects.GetBattleState();

        State.SelectedUnit.OnPreparedForCombat += OnAttackerPrepared;

        Grid.Unit attacker = State.SelectedUnit;
        Grid.Unit defender = State.AttackTarget;

        attacker.PrepareForCombat(MathUtils.CardinalDirection.W);
        defender.PrepareForCombat(MathUtils.CardinalDirection.E);
    }

    void OnAttackerPrepared() {
        Animator.SetTrigger("units_prepared");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State.SelectedUnit.OnPreparedForCombat -= OnAttackerPrepared;
    }
}