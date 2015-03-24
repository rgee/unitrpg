using UnityEngine;
using System.Collections;

public class LevelUpIntro : StateMachineBehaviour {

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetTrigger("level_up_intro_complete");
	}
}
