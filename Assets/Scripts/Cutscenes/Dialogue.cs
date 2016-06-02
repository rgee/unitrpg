using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IDialogueController))]
[RequireComponent(typeof(DialogueTextAnimator))]
public class Dialogue : MonoBehaviour {
    public TextAsset SourceFile;
    public event Action OnComplete;
    public bool AutoStart = false;

    private IDialogueController _controller;
    private DialogueTextAnimator _textAnimator;
    public Models.Dialogue.Cutscene Model;

    private int _deckIndex = -1;
    private int _cardIndex;
    private bool _running;

    private void Awake() {
        _controller = GetComponent<IDialogueController>();
        _textAnimator = GetComponent<DialogueTextAnimator>();
        if (SourceFile != null) {
            Model = Models.Dialogue.DialogueUtils.ParseFromJson(SourceFile.text);
        }
    }

    private void Start() {
        if (AutoStart) {
            Begin();
        }
    }

    public void Begin() {
        StartCoroutine(Initialize());
    }

    private void Update() {
        if (!_running) {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            NextCard();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            StartCoroutine(End());
        }
    }

    private IEnumerator End() {
        yield return StartCoroutine(_controller.End());
        _running = false;
        if (OnComplete != null) {
            OnComplete();
        }
    }

    private IEnumerator Initialize() {
        yield return StartCoroutine(_controller.Initialize(Model));
        _running = true;
        NextDeck();
    } 

    public void NextCard() {

        // If we've passed the last card in the deck, look for the next deck.
        var currentDeck = Model.Decks[_deckIndex];
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
        if (_deckIndex >= Model.Decks.Count) {
            StartCoroutine(End());
            return;
        }

        // Start the next deck.
        SetSpeaker();
        NextCard();
    }

    private void SetSpeaker() {
        var deck = Model.Decks[_deckIndex];
        _controller.ChangeSpeaker(deck.Speaker);
        _textAnimator.ChangeSpeaker(deck.Speaker);
    }
}