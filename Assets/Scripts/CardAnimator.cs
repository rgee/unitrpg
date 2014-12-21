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
		foreach (string sentence in card.Sentences) {
			foreach (char letter in sentence) {
				textObject.text += letter;
				yield return new WaitForSeconds(0.1f);
			}

			textObject.text += " ";

			Debug.Log ("Says: " + sentence);
		}
		complete = true;
	}
}
