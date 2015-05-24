using UnityEngine;

public class DefenderDies : StateMachineBehaviour {
    private GameObject DefenderObject;

    public override void OnStateEnter(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
            stateMachine.SetTrigger("death_complete");
        DefenderObject = FightExecutionObjects.GetState().Defender.gameObject;
    }

    public override void OnStateUpdate(Animator stateMachine, AnimatorStateInfo stateInfo, int layerIndex) {
        // Weirdly, GameObject overrides the '==' operator to return 'true' when compared to 'null'
        // after it has been destroyed.
        if (DefenderObject == null) {
        }
    }
}