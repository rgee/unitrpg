using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AttackerAttacks : StateMachineBehaviour {
    private FightExcecutionState State;
    private ScreenShaker ScreenShaker;
    private Animator StateMachine;

    public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        State = FightExecutionObjects.GetState();
        ScreenShaker = FightExecutionObjects.GetScreenShaker();
        StateMachine = stateMachine;

        FightPhaseResult phase = State.Result.InitialAttack;
        Grid.Unit attacker = GetUnitComponent(State.Attacker);
        Grid.Unit defender = GetUnitComponent(State.Defender);

        attacker.OnHitConnect += ShakeScreen;

        FightPhaseExecutor phaseExecutor = new FightPhaseExecutor(attacker, defender, phase.AttackerHits);
        phaseExecutor.OnComplete += TransitionToComplete;
        phaseExecutor.OnTargetDied += TransitionToDead;

        phaseExecutor.Run();
    }

    private Grid.Unit GetUnitComponent(GameObject gameObject) {
        return gameObject.GetComponent<Grid.Unit>();
    }

    private void TransitionToDead(object sender, EventArgs args) {
        StateMachine.SetTrigger("defender_dead");
    }

    private void TransitionToComplete(object sender, EventArgs args) {
        StateMachine.SetTrigger("attacker_attack_complete");
    }

    private void ShakeScreen(object sender, Grid.Unit.AttackConnectedEventArgs args) {
        if (args.hit.Crit) {
            ScreenShaker.CritShake();
        } else {
            ScreenShaker.Shake();
        }
    }

}
