using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneDirector : MonoBehaviour {
    private readonly Dictionary<string, GameObject> portraits = new Dictionary<string, GameObject>();
    private CardAnimator cardAnimator;
    private bool complete;
    public Cutscene Cutscene;
    private DeckAnimator deckAnimator;
    private int deckIndex;
    private SceneFader sceneTransitioner;
    private Text text;

    private void Start() {
        cardAnimator = GetComponent<CardAnimator>();
        deckAnimator = GetComponent<DeckAnimator>();
        text = gameObject.GetComponentInChildren<Text>();
        deckAnimator.animator = cardAnimator;

        sceneTransitioner = GameObject.FindGameObjectWithTag("SceneTransitioner").GetComponent<SceneFader>();

        // Grab handles to all the portraits of the actors in the scene.
        foreach (var actor in Cutscene.actors) {
            Debug.Log(actor.name);

            var portrait = transform.FindChild("Canvas")
                                    .FindChild("Portraits")
                                    .FindChild(actor.name);
            if (portrait != null) {
                portraits.Add(actor.name, portrait.gameObject);
            }
        }

        // Kick the first deck off immediately.
        StartCoroutine(Begin());
    }

    private IEnumerator Begin() {
        yield return StartCoroutine(sceneTransitioner.FadeIn());
        yield return new WaitForSeconds(0.3f);
        Advance();
    }

    private void Update() {
        if (complete) {
            return;
        }

        // Advance to the next deck of dialogue cards if the user presses
        // the space bar, and the current animation is complete.
        if (Input.GetKeyDown(KeyCode.Space) && cardAnimator.complete) {
            deckIndex++;

            if (deckIndex >= Cutscene.decks.Length) {
                StartCoroutine(TransitionToNextScene());
            } else {
                Advance();
            }
        }
    }

    private IEnumerator TransitionToNextScene() {
        complete = true;
        yield return StartCoroutine(sceneTransitioner.FadeOut());
        Application.LoadLevel(Cutscene.nextScene);
    }

    private void Advance() {
        var newDeck = Cutscene.decks[deckIndex];

        // Activate the new speaker and deactivate all others.
        var speaker = portraits[newDeck.speaker].GetComponent<CutscenePortrait>();
        speaker.Activate();
        speaker.ChangeEmotion(newDeck.emotionType);

        var previousDeck = deckAnimator.deck;
        if (portraits.ContainsKey(previousDeck.speaker)) {
            var previousSpeaker = portraits[previousDeck.speaker];
            previousSpeaker.GetComponent<CutscenePortrait>().ChangeEmotion(EmotionType.DEFAULT);
        }

        foreach (var pair in portraits) {
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