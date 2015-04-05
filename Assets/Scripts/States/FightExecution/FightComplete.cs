using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FightComplete : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        FightExcecutionState state = FightExecutionObjects.GetState();
        state.Attacker.GetComponent<Grid.Unit>().ReturnToRest();
        state.Defender.GetComponent<Grid.Unit>().ReturnToRest();
        state.Complete = true;
    }
}
