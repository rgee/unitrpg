using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AttackerDies : StateMachineBehaviour {

    private GameObject AttackerObject;

    public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        AttackerObject = FightExecutionObjects.GetState().Attacker.gameObject;
    }

    public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        // Weirdly, GameObject overrides the '==' operator to return 'true' when compared to 'null'
        // after it has been destroyed.
        if (AttackerObject == null) {
            stateMachine.SetTrigger("death_complete");
        }
    }
}
