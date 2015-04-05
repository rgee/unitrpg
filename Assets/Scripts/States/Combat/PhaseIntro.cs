using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhaseIntro : StateMachineBehaviour {

	public GameObject PhaseTextPrefab;
    public string TransitionTriggerName;
    public float SlideSeconds = 0.5f;
    public float CenterPauseSeconds = 0.5f;

	private Grid.UnitManager UnitManager;
	private GridCameraController CameraController;
    private GameObject currentPhaseText;
    private GameObject phaseTextCanvas;
    private PhaseText phaseTextBehavior;
    private RectTransform canvasTransform;
    private RectTransform textTransform;
    private bool textMoved;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		UnitManager = GameObject.Find("Unit Manager").GetComponent<Grid.UnitManager>();
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

		Vector2 center = new Vector2(canvasTransform.rect.width / 2, 0);
		Vector2 offscreen = new Vector2(canvasTransform.rect.width + textTransform.rect.width, 0);
		PhaseText mover = currentPhaseText.GetComponent<PhaseText>();

		PhaseTextFlyByCommand flyBy = new PhaseTextFlyByCommand(center, offscreen, SlideSeconds, CenterPauseSeconds);
		mover.MoveThroughScreen(flyBy, () => {
			animator.SetTrigger(TransitionTriggerName);
		});
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
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
