using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FightComplete : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        FightExcecutionState state = FightExecutionObjects.GetState();
        if (state.Attacker != null) { 
            state.Attacker.GetComponent<Grid.Unit>().ReturnToRest();
        }

        if (state.Defender != null) {
            state.Defender.GetComponent<Grid.Unit>().ReturnToRest();
        }
        state.Complete = true;
    }
}
