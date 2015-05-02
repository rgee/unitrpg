using System;
using UnityEngine;

public class AttackerAttacks : StateMachineBehaviour {
    private FightExcecutionState State;
    private Animator StateMachine;

    public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        State = FightExecutionObjects.GetState();
        StateMachine = stateMachine;

        var phase = State.Result.InitialAttack;
        var attacker = GetUnitComponent(State.Attacker);
        var defender = GetUnitComponent(State.Defender);

        var phaseExecutor = new FightPhaseExecutor(attacker, defender, phase.AttackerHits);
        phaseExecutor.OnComplete += TransitionToComplete;
        phaseExecutor.OnTargetDied += TransitionToDead;

        phaseExecutor.Run();
    }

    private static Grid.Unit GetUnitComponent(GameObject gameObject) {
        return gameObject.GetComponent<Grid.Unit>();
    }

    private void TransitionToDead(object sender, EventArgs args) {
        StateMachine.SetTrigger("defender_dead");
    }

    private void TransitionToComplete(object sender, EventArgs args) {
        StateMachine.SetTrigger("attacker_attack_complete");
    }
}