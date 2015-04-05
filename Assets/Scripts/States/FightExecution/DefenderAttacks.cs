using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DefenderAttacks : StateMachineBehaviour {
    
    private FightExcecutionState State;
    private Animator StateMachine;

    public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        State = FightExecutionObjects.GetState();
        StateMachine = stateMachine;

        FightPhaseResult phase = State.Result.CounterAttack;
        Grid.Unit attacker = GetUnitComponent(State.Defender);
        Grid.Unit defender = GetUnitComponent(State.Attacker);

        FightPhaseExecutor phaseExecutor = new FightPhaseExecutor(attacker, defender, phase.AttackerHits);
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
