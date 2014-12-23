using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutscenePortrait : MonoBehaviour {

	private Image portrait;

	private static Color DEACTIVATED_COLOR = new Color(0.5f, 0.5f, 0.5f);
	private static Color ACTIVATED_COLOR = Color.white;
	private static float TINT_DURATION_SECONDS = 0.5f;

	void Start () {
		portrait = GetComponent<Image>();
	}

	public void Activate() {
		StartCoroutine(TintToColor(ACTIVATED_COLOR, TINT_DURATION_SECONDS));
	}

	public void Deactivate() {
		StartCoroutine(TintToColor(DEACTIVATED_COLOR, TINT_DURATION_SECONDS));
	}

	private IEnumerator TintToColor(Color color, float time) {

		float elapsedTime = 0;
		Color startColor = portrait.color;
		while (elapsedTime < time) {
			portrait.color = Color.Lerp(startColor, color, (elapsedTime/time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
