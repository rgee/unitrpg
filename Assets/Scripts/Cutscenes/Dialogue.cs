using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(IDialogueController))]
[RequireComponent(typeof(DialogueTextAnimator))]
public class Dialogue : MonoBehaviour {
    public TextAsset SourceFile;

    private IDialogueController _controller;
    private DialogueTextAnimator _textAnimator;
    private Models.Dialogue.Cutscene _model;

    private int _deckIndex = -1;
    private int _cardIndex;
    private bool _initialized;

    private void Awake() {
        _controller = GetComponent<IDialogueController>();
        _textAnimator = GetComponent<DialogueTextAnimator>();
        _model = Models.Dialogue.DialogueUtils.ParseFromJson(SourceFile.text);
    }

    private void Start() {
        StartCoroutine(Initialize());
    }

    private void Update() {
        if (!_initialized) {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            NextCard();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine(_controller.End());
        }
    }

    private IEnumerator Initialize() {
        yield return StartCoroutine(_controller.Initialize(_model));
        _initialized = true;
        NextDeck();
    } 

    private void NextCard() {

        // If we've passed the last card in the deck, look for the next deck.
        var currentDeck = _model.Decks[_deckIndex];
        if (_cardIndex >= currentDeck.Cards.Count) {
            NextDeck();
        } else {

            // If there are cards remaining, start the next card.
            var nextCard = currentDeck.Cards[_cardIndex];
            _cardIndex++;

            foreach (var entry in nextCard.EmotionalResponses) {
                _controller.ChangeEmotion(entry.Key, entry.Value);
            }
            
            _textAnimator.AnimateCard(nextCard);
        }
    }

    private void NextDeck() {
        _deckIndex++;
        _cardIndex = 0;

        // If we've reached the end of the decks, end the dialogue.
        if (_deckIndex >= _model.Decks.Count) {
            StartCoroutine(_controller.End());
            return;
        }

        // Start the next deck.
        SetSpeaker();
        NextCard();
    }

    private void SetSpeaker() {
        var deck = _model.Decks[_deckIndex];
        _controller.ChangeSpeaker(deck.Speaker);
        _textAnimator.ChangeSpeaker(deck.Speaker);
    }
}