using UnityEngine;
using System.Collections;

public class Tray : MonoBehaviour {

	public float xThresholdPct;
	private bool doneOnce;

	private TrayPortrait[] portraits;
	private Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
		portraits = GetComponentsInChildren<TrayPortrait>();
	}

	void Update () {
		float mouseX = Input.mousePosition.x;
		if (mouseX < 0 || mouseX > Screen.width) {
			return;
		}

		float pctToRight = mouseX / Screen.width;

		bool shouldOpen = pctToRight > xThresholdPct;
		animator.SetBool("visible", shouldOpen);

		foreach (TrayPortrait portrait in portraits) {
			if (portrait.character.isDead) {
				print ("disabling");
				portrait.gameObject.SetActive(false);
			}
		}
	}
}
