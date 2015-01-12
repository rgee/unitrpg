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

                // Do not allow the user to skip a card that has already been completed.
                if (!animator.complete) {
                    animator.Skip();
                    NextCard(); 
                }
			} else if (Input.GetKeyDown(KeyCode.Space)) {
				if (animator.complete) {
                    // Completing a card should wipe the textbox to make room for the next.
                    textObject.text = "";
					StartAnimation();
				} 
			}
		}
	}

    /// <summary>
    /// Begin animating the current deck.
    /// </summary>
	public void StartAnimation() {
		StartCoroutine(AnimateCurrentCard());
	}

    /// <summary>
    /// Set up the state to look at the next card in the list.
    /// </summary>
    private void NextCard() {
        cardIdx++;
        if (cardIdx >= deck.cards.Length) {
            complete = true;
        }
    }

	private IEnumerator AnimateCurrentCard() {
        if (cardIdx >= deck.cards.Length) {
            Debug.Log("Shit's fuct at: " + cardIdx);
            yield return null;
        }
		Models.Card currentCard = deck.cards[cardIdx];
		animator.textObject = textObject;
		animator.card = currentCard;
		animator.Reset();
		yield return animator.StartAnimation();

        NextCard();
	}
}
