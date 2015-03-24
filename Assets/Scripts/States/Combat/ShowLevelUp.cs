using UnityEngine;
using System.Collections;

public class ShowLevelUp : StateMachineBehaviour {
	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetTrigger("level_up_confirmed");
	}
}
