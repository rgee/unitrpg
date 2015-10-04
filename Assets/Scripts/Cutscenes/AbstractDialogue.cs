using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Dialogue;
using UnityEngine;

[RequireComponent(typeof(DialogueTextAnimator))]
public abstract class AbstractDialogue : MonoBehaviour {
    private DialogueTextAnimator _animator;
    private Cutscene _dialogue;


    public virtual Cutscene Dialogue {
        get {
            return _dialogue;
        }

        set {
            _dialogue = value;
            _deckIndex = 0;
            _cardIndex = 0;
        }
    }

    private int _deckIndex = -1;
    private int _cardIndex = -1;

    protected abstract void ChangeEmotion(Models.EmotionType emotion);
    public abstract void SkipDialogue();

    protected virtual void Start() {
        _animator = GetComponent<DialogueTextAnimator>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ShowNextCard();
        }
    }

    public void ShowNextCard() {
        if (Dialogue == null) {
            return;
        }
        
        if (Dialogue.Decks.Count <= _deckIndex) {
            SkipDialogue();
            return;
        }

        var currentDeck = Dialogue.Decks[_deckIndex];
        ChangeSpeaker(currentDeck.Speaker);

        if (currentDeck.Cards.Count <= _cardIndex) {
            _deckIndex++;
            _cardIndex = 0;
            ShowNextCard();
        } else {
            var nextCard = currentDeck.Cards[_cardIndex];
            _cardIndex++;

            ChangeEmotion(nextCard.Emotion);
            _animator.AnimateCard(nextCard);
        }
    }


    protected virtual void ChangeSpeaker(string speakerName) {
        _animator.ChangeSpeaker(speakerName);
    }

    public void SkipToEndOfCard() {
        throw new NotImplementedException();
    }
}
