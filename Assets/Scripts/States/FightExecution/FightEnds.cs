using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FightEnds : StateMachineBehaviour {
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        CombatObjects.GetCameraController().Unlock();
    }
}
