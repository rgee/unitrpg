using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerPhaseIntro : StateMachineBehaviour {
	public GameObject PhaseTextPrefab;
    public float SlideSeconds = 0.5f;
    public float CenterPauseSeconds = 0.5f;

    private GameObject currentPhaseText;
    private GameObject phaseTextCanvas;
    private PhaseText phaseTextBehavior;
    private RectTransform canvasTransform;
    private RectTransform textTransform;
    private bool textMoved;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        phaseTextCanvas = Instantiate(PhaseTextPrefab);
        currentPhaseText = phaseTextCanvas.transform.FindChild("Phase Text").gameObject;

        // Anchor the text to the left side.
        textTransform = currentPhaseText.GetComponent<RectTransform>();
        canvasTransform = phaseTextCanvas.GetComponent<RectTransform>();

        textTransform.anchorMax = new Vector2(0f, 0.5f);
        textTransform.anchorMin = new Vector2(0f, 0.5f);

        // Position the text all the way off the left hand side of the screen.
        textTransform.anchoredPosition = new Vector3(-textTransform.rect.width/2, 0, textTransform.position.z);

        // Jank...can't start a coroutine from StateMachineBehaviours
        phaseTextBehavior = phaseTextCanvas.GetComponent<PhaseText>();
        phaseTextBehavior.StartCoroutine(MoveText());
	}

    private IEnumerator MoveText() {
        // Move the text to the center, pause for a bit, then move it off the right side.
        yield return phaseTextBehavior.StartCoroutine(MoveTextTo(textTransform.anchoredPosition, new Vector2(canvasTransform.rect.width / 2, 0), SlideSeconds));
        yield return new WaitForSeconds(CenterPauseSeconds);
        yield return phaseTextBehavior.StartCoroutine(MoveTextTo(textTransform.anchoredPosition, new Vector2(canvasTransform.rect.width + textTransform.rect.width, 0), SlideSeconds));
        textMoved = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (textMoved) {
            animator.SetTrigger("phase_text_complete");
        }
    }

    private IEnumerator MoveTextTo(Vector2 source, Vector2 destination, float duration) {
        float startTime = Time.time;
        while (Time.time < startTime + duration) {
            textTransform.anchoredPosition = Vector2.Lerp(source, destination, (Time.time - startTime) / duration);
            yield return null;
        }
        textTransform.anchoredPosition = destination;
        Debug.Log("Final Position: " + textTransform.anchoredPosition);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Destroy(phaseTextCanvas);        	
	}
}
