using UnityEngine;
using System.Collections;

public class TempGameOver : StateMachineBehaviour {
	public GameObject GameOverOverlayPrefab;
	private GameObject GameOverOverlay;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GameOverOverlay = Instantiate(GameOverOverlayPrefab) as GameObject;
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Destroy(GameOverOverlay);
	}
}
