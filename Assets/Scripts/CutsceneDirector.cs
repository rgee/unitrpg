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
			Debug.Log(actor.name);

			Transform portrait = transform.FindChild("Canvas")
									      .FindChild("Portraits")
					  					  .FindChild(actor.name);
			if (portrait != null) {
				portraits.Add(actor.name, portrait.gameObject);
			}
		}

		// Kick the first deck off immediately.
		advance();
	}
	
	void Update () {
		// Advance to the next deck of dialogue cards if the user presses
		// the space bar, and the current animation is complete.
		if (Input.GetKeyDown(KeyCode.Space) && cardAnimator.complete) {
			deckIndex++;
			advance();
		}
	}

	private void advance() {
		Models.Deck newDeck = Cutscene.decks[deckIndex];

		// Activate the new speaker and deactivate all others.
		portraits[newDeck.speaker].GetComponent<CutscenePortrait>().Activate();
		foreach (KeyValuePair<string, GameObject> pair in portraits) {
			if (pair.Key != newDeck.speaker) {
				pair.Value.GetComponent<CutscenePortrait>().Deactivate();
			}
		}

		// Set up the text box and the animator for the next deck
		text.text = "";
		deckAnimator.textObject = text;
		deckAnimator.deck = newDeck;
		deckAnimator.Reset();

		// Begin showing the next deck
		deckAnimator.StartAnimation();
	}
}
