using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneDirector : MonoBehaviour {
	public Models.Cutscene Cutscene;

	private CardAnimator cardAnimator;
	private DeckAnimator deckAnimator;
	private Text text;
	private int deckIndex;

	void Start () {
		cardAnimator = GetComponent<CardAnimator>();
		deckAnimator = GetComponent<DeckAnimator>();
		text = gameObject.GetComponentInChildren<Text>();
		deckAnimator.animator = cardAnimator;

		// Kick the first deck off immediately.
		advance();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space) && cardAnimator.complete) {
			deckIndex++;
			advance();
		}
	}

	private void advance() {
		Models.Deck newDeck = Cutscene.decks[deckIndex];
		text.text = "";
		deckAnimator.textObject = text;
		deckAnimator.deck = newDeck;
		deckAnimator.Reset();
		deckAnimator.StartAnimation();
	}
}
