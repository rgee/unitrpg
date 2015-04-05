using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FightBegins : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        CombatObjects.GetCameraController().Lock();
        animator.SetTrigger("fight_initialized");
    }
}
