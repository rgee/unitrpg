using System;
using UnityEngine;

public class DefenderAttacks : StateMachineBehaviour {
    private FightExcecutionState State;
    private Animator StateMachine;

    public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        State = FightExecutionObjects.GetState();
        StateMachine = stateMachine;

        var phase = State.Result.CounterAttack;
        var attacker = GetUnitComponent(State.Defender);
        var defender = GetUnitComponent(State.Attacker);

        var phaseExecutor = new FightPhaseExecutor(attacker, defender, phase.AttackerHits);
        phaseExecutor.OnComplete += TransitionToComplete;
        phaseExecutor.OnTargetDied += TransitionToDead;

        phaseExecutor.Run();
    }

    private void TransitionToDead(object snder, EventArgs args) {
        StateMachine.SetTrigger("attacker_dead");
    }

    private void TransitionToComplete(object sender, EventArgs args) {
        StateMachine.SetTrigger("defender_attack_complete");
    }

    private static Grid.Unit GetUnitComponent(GameObject gameObject) {
        return gameObject.GetComponent<Grid.Unit>();
    }
}