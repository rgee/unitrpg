using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeckAnimator : MonoBehaviour {
	public Models.Deck deck;
	public Text textObject;
	public CardAnimator animator;

	public bool complete;
	
	public void Reset() {
		complete = false;
	}

	public Coroutine StartAnimation() {
		return StartCoroutine(Animate());
	}

	IEnumerator Animate() {
		animator.textObject = textObject;
		foreach (Models.Card card in deck.cards) {
			animator.card = card;
			animator.Reset();
			yield return animator.StartAnimation();
		}
		complete = true;
	}
}
