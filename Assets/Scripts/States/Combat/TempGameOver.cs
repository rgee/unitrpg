using UnityEngine;

public class TempGameOver : StateMachineBehaviour {
    private GameObject GameOverOverlay;
    public GameObject GameOverOverlayPrefab;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameOverOverlay = Instantiate(GameOverOverlayPrefab);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Destroy(GameOverOverlay);
    }
}