using System;
using UnityEngine;

public class AttackerAttacks : StateMachineBehaviour {
    private FightExcecutionState _state;
    private Animator _stateMachine;

    public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        _state = FightExecutionObjects.GetState();
        _stateMachine = stateMachine;

        var phase = _state.Result.InitialAttack;
        var attacker = GetUnitComponent(_state.Attacker);
        var defender = GetUnitComponent(_state.Defender);

        var hitIndex = stateMachine.GetInteger("attack count");

        var currentHit = phase.AttackerHits[hitIndex];

        var phaseExecutor = new FightPhaseExecutor(attacker, defender, currentHit);
        phaseExecutor.OnComplete += TransitionToComplete;
        phaseExecutor.OnTargetDied += TransitionToDead;

        phaseExecutor.Run();
    }

    private static Grid.Unit GetUnitComponent(GameObject gameObject) {
        return gameObject.GetComponent<Grid.Unit>();
    }

    private void TransitionToDead(object sender, EventArgs args) {
        _stateMachine.SetTrigger("defender_dead");
    }

    private void TransitionToComplete(object sender, EventArgs args) {
        var nextAttackIndex = _stateMachine.GetInteger("attack count") + 1;
        _stateMachine.SetInteger("attack count", nextAttackIndex);

        var phase = _state.Result.InitialAttack;
        _stateMachine.SetBool("attacker_finished", nextAttackIndex >= phase.AttackerHits.Count);

        _stateMachine.SetTrigger("attacker_attack_complete");
    }
}