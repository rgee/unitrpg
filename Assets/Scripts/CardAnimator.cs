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
		foreach (Models.DialogueLine line in card.lines) {

			// Show the sentence character at a time
			foreach (char letter in line.text) {
				textObject.text += letter;
				yield return new WaitForSeconds(0.02f);
			}

			// Enforce spaces between sentences
			textObject.text += " ";

			float delay = 0.4f;
			if (line.delayOverride > 0f) {
				delay = line.delayOverride;
			}

			yield return new WaitForSeconds(delay);

			Debug.Log ("Says: " + line.text);
			sentenceIndex++;
		}
		complete = true;
	}
}
