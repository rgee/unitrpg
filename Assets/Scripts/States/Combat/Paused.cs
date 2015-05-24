using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Paused : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Time.timeScale = 0;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Time.timeScale = 1;
    }
}
