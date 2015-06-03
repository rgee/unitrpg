using Grid;
using UnityEngine;

public class PhaseIntro : StateMachineBehaviour {
    private GridCameraController CameraController;
    private RectTransform canvasTransform;
    public float CenterPauseSeconds = 0.5f;
    private GameObject currentPhaseText;
    private PhaseText phaseTextBehavior;
    private GameObject phaseTextCanvas;
    public GameObject PhaseTextPrefab;
    public float SlideSeconds = 0.5f;
    private bool textMoved;
    private RectTransform textTransform;
    public string TransitionTriggerName;
    private UnitManager UnitManager;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UnitManager = GameObject.Find("Unit Manager").GetComponent<UnitManager>();
        CameraController = GameObject.Find("Grid Camera/Main Camera").GetComponent<GridCameraController>();

        LockControls();
        phaseTextCanvas = Instantiate(PhaseTextPrefab);
        currentPhaseText = phaseTextCanvas.transform.FindChild("Phase Text").gameObject;

        // Anchor the text to the left side.
        textTransform = currentPhaseText.GetComponent<RectTransform>();
        canvasTransform = phaseTextCanvas.GetComponent<RectTransform>();

        textTransform.anchorMax = new Vector2(0f, 0.5f);
        textTransform.anchorMin = new Vector2(0f, 0.5f);

        // Position the text all the way off the left hand side of the screen.
        textTransform.anchoredPosition = new Vector3(-textTransform.rect.width/2, 0, textTransform.position.z);

        var center = new Vector2(canvasTransform.rect.width/2, 0);
        var offscreen = new Vector2(canvasTransform.rect.width + textTransform.rect.width, 0);
        var mover = currentPhaseText.GetComponent<PhaseText>();

        var flyBy = new PhaseTextFlyByCommand(center, offscreen, SlideSeconds, CenterPauseSeconds);
        mover.MoveThroughScreen(flyBy, () => { animator.SetTrigger(TransitionTriggerName); });
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Uncomment to make the battle end immediately.
        animator.SetTrigger("battle_won");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UnlockControls();
        Destroy(phaseTextCanvas);
    }

    private void LockControls() {
        UnitManager.Lock();
        CameraController.Lock();
    }

    private void UnlockControls() {
        UnitManager.Unlock();
        CameraController.Unlock();
    }
}