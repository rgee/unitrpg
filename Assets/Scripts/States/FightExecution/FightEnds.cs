using UnityEngine;

public class FightEnds : StateMachineBehaviour {
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //CombatObjects.GetCameraController().Unlock();
    }
}