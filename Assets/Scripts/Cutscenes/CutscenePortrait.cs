using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CutscenePortrait : MonoBehaviour {
	private static Color DEACTIVATED_COLOR = new Color(0.5f, 0.5f, 0.5f);
	private static Color ACTIVATED_COLOR = Color.white;
	private static float TINT_DURATION_SECONDS = 0.5f;

  	[System.Serializable]
	public class Emotion {
		public Sprite face;
		public Models.EmotionType type;
	}
	
	public List<Emotion> Emotions;

	private Image Portrait;

	private Image Head;
	private Image Body;

	public Models.EmotionType currentEmotion = Models.EmotionType.DEFAULT;

	void Start() {
		Portrait = GetComponent<Image>();
		Head = transform.FindChild("Head").gameObject.GetComponent<Image>();
		Head.sprite = FindSpriteForEmotion(Models.EmotionType.DEFAULT);

		Body = transform.FindChild("Body").gameObject.GetComponent<Image>();
	}
	
	void Update () {
		Head.sprite = FindSpriteForEmotion (currentEmotion);
	}

	public void ChangeEmotion(Models.EmotionType type) {
		Head.sprite = FindSpriteForEmotion(type);
	}

	private Sprite FindSpriteForEmotion(Models.EmotionType type) {
		foreach (Emotion emo in Emotions) {
			if (emo.type == type) {
				return emo.face;
			}
		}

		return null;
	}

	public void Activate() {
		StartCoroutine(TintToColor(DEACTIVATED_COLOR, ACTIVATED_COLOR, TINT_DURATION_SECONDS));
	}

	public void Deactivate() {
		StartCoroutine(TintToColor(ACTIVATED_COLOR, DEACTIVATED_COLOR, TINT_DURATION_SECONDS));
	}

	private IEnumerator TintToColor(Color startColor, Color color, float time) {

		float elapsedTime = 0;
		while (elapsedTime < time) {
			Color nextColor = Color.Lerp(startColor, color, (elapsedTime/time));
			Body.color = nextColor;
			Head.color = nextColor;

			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}
}
