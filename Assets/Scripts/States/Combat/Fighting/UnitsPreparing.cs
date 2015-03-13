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

        MathUtils.CardinalDirection attackerDirection = MathUtils.DirectionTo(attacker.gridPosition, defender.gridPosition);
        MathUtils.CardinalDirection defenderDirection = MathUtils.DirectionTo(defender.gridPosition, attacker.gridPosition);

        attacker.PrepareForCombat(attackerDirection);
        defender.PrepareForCombat(defenderDirection);
    }

    void OnAttackerPrepared() {
        Animator.SetTrigger("units_prepared");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State.SelectedUnit.OnPreparedForCombat -= OnAttackerPrepared;
    }
}