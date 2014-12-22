using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardAnimator : MonoBehaviour {
	public Models.Card card;
	public Text textObject;
	public bool complete;
	
	public void Reset() {
		complete = false;
	}

	public Coroutine StartAnimation() {
		return StartCoroutine(Animate());
	}

	IEnumerator Animate() {
		int sentenceIndex = 0;
		foreach (string sentence in card.Sentences) {

			foreach (char letter in sentence) {
				textObject.text += letter;
				yield return new WaitForSeconds(0.02f);
			}

			textObject.text += " ";
			if (sentenceIndex <= card.Delays.Length - 1) {
				float delay = card.Delays[sentenceIndex];
				if (delay > 0f) {
					yield return new WaitForSeconds(delay);
				} else {
					yield return new WaitForSeconds(0.4f);
				}
			} else {
				yield return new WaitForSeconds(0.4f);
			}

			Debug.Log ("Says: " + sentence);
			sentenceIndex++;
		}
		complete = true;
	}
}
