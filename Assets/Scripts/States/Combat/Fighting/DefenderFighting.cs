using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class DefenderFighting : StateMachineBehaviour {
    private Animator Animator;
    private BattleState State;
	private int numAttacks;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;
        State = CombatObjects.GetBattleState();

        Grid.Unit defender = State.AttackTarget;

        defender.OnAttackComplete += OnAttackComplete;

		FightPhaseResult firstAttack = State.FightResult.CounterAttack;
		Hit firstHit = firstAttack.AttackerHits[0];
		defender.Attack(State.SelectedUnit, firstHit, firstAttack.DefenderDies);

		if (firstHit.Missed) {
			State.SelectedUnit.Dodge();
		}
    }

    void OnAttackComplete() {
		FightResult result = State.FightResult;
		State.SelectedUnit.TakeDamage(result.CounterAttack.AttackerHits[numAttacks].Damage);
		numAttacks++;

		if (!State.SelectedUnit.IsAlive() || numAttacks >= result.CounterAttack.AttackerHits.Count) {
			State.AttackTarget.ReturnToRest();
			State.SelectedUnit.ReturnToRest();
			Animator.SetTrigger("fight_completed");
		} else {
			Hit nextHit = result.CounterAttack.AttackerHits[numAttacks];
			State.AttackTarget.Attack(State.SelectedUnit, nextHit, result.CounterAttack.DefenderDies);
			if (nextHit.Missed) {
				State.SelectedUnit.Dodge();
			}
		}
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State.AttackTarget.OnAttackComplete -= OnAttackComplete;
		numAttacks = 0;
    }
}