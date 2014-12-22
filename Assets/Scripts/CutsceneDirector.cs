using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CutsceneDirector : MonoBehaviour {
	public Models.Cutscene Cutscene;

	private CardAnimator cardAnimator;
	private DeckAnimator deckAnimator;
	private Text text;
	private int deckIndex;
	private Dictionary<string, GameObject> portraits = new Dictionary<string, GameObject>();

	void Start () {
		cardAnimator = GetComponent<CardAnimator>();
		deckAnimator = GetComponent<DeckAnimator>();
		text = gameObject.GetComponentInChildren<Text>();
		deckAnimator.animator = cardAnimator;

		// Grab handles to all the portraits of the actors in the scene.
		foreach (CutsceneActor actor in Cutscene.actors) {
			Transform portrait = transform.FindChild(actor.name);
			if (portrait != null) {
				portraits.Add(actor.name, portrait.gameObject);
			}
		}

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
