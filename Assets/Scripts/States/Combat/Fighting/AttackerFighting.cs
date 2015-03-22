using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AttackerFighting : StateMachineBehaviour {

    private Animator Animator;
    private BattleState State;
	private int numAttacks;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;

        State = CombatObjects.GetBattleState();

		FightPhaseResult result = State.FightResult.InitialAttack;
		Hit firstHit = result.AttackerHits[0];

        Grid.Unit unit = State.SelectedUnit;
		unit.OnHitConnect += OnHitConnect;
        unit.OnAttackComplete += OnAttackComplete;
        unit.Attack(State.AttackTarget, firstHit);

		if (firstHit.Missed) {
			State.AttackTarget.Dodge();
		}
    }

	void OnHitConnect(object sender, Grid.Unit.AttackConnectedEventArgs args) {
		ScreenShaker shaker = CombatObjects.GetCameraController().gameObject.GetComponent<ScreenShaker>();
		if (args.hit.Crit) {
			shaker.CritShake();
		} else {
			shaker.Shake();
		}
	}

    void OnAttackComplete() {
		FightResult result = State.FightResult;
		State.AttackTarget.TakeDamage(result.InitialAttack.AttackerHits[numAttacks].Damage);
		numAttacks++;

		if (!State.AttackTarget.IsAlive()) {
			Animator.SetTrigger("defender_died");
		} else {
			if (numAttacks >= result.InitialAttack.AttackerHits.Count) {
				Animator.SetTrigger("defender_survived");
			} else {
				Hit nextHit = result.InitialAttack.AttackerHits[numAttacks];

				State.SelectedUnit.Attack(State.AttackTarget, nextHit);
				if (nextHit.Missed) {
					State.AttackTarget.Dodge();
				}
			}	
		}
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State.SelectedUnit.OnAttackComplete -= OnAttackComplete;
		State.SelectedUnit.OnHitConnect -= OnHitConnect;
		numAttacks = 0;
    }
}
