using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutscenePortrait : MonoBehaviour {

	private Image portrait;

	void Start () {
		portrait = GetComponent<Image>();
	}

	public void Activate() {
		StartCoroutine(TintToColor(Color.white, 0.5f));
	}

	public void Deactivate() {
		StartCoroutine(TintToColor(new Color(0.5f, 0.5f, 0.5f), 0.5f));
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
