using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeckAnimator : MonoBehaviour {
	public Models.Deck deck;
	public Text textObject;

	[HideInInspector]
	public CardAnimator animator;

	public bool complete;
	private int cardIdx;
	
	public void Reset() {
		complete = false;
		cardIdx = 0;
	}

	public void Update() {
		if (!complete) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				cardIdx = 0;
				textObject.text = "";
				animator.Reset ();

				while (!complete) {
					Models.Card currentCard = deck.cards[cardIdx];
					animator.textObject = textObject;
					animator.card = currentCard;

					animator.Skip();

					cardIdx++;
					if (cardIdx >= deck.cards.Length) {
						complete = true;
					}
				}
			} else if (Input.GetKeyDown(KeyCode.Space)) {
				if (animator.complete) {
					StartAnimation();
				} 
			}
		}
	}

	public void StartAnimation() {
		StartCoroutine(AnimateCurrentCard());
	}

	private IEnumerator AnimateCurrentCard() {
		Models.Card currentCard = deck.cards[cardIdx];
		animator.textObject = textObject;
		animator.card = currentCard;
		animator.Reset();
		yield return animator.StartAnimation();

		cardIdx++;
		if (cardIdx >= deck.cards.Length) {
			complete = true;
		}
	}

	IEnumerator Animate() {

		Models.Card newCard = deck.cards[cardIdx];

		// Because a deck is just a series of cards with a speaker
		// just animate them in order.
		animator.textObject = textObject;
		foreach (Models.Card card in deck.cards) {
			animator.card = card;
			animator.Reset();
			yield return animator.StartAnimation();
		}
		complete = true;
	}
}
