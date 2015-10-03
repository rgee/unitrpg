using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Dialogue;
using UnityEngine;

[RequireComponent(typeof(DialogueAnimator))]
public abstract class AbstractDialogueController : DialogueController {
    private DialogueAnimator _animator;
    public override Models.Dialogue.Cutscene Dialogue {
        get { return base.Dialogue; }
        set
        {
            base.Dialogue = value;
            _deckIndex = 0;
            _cardIndex = 0;
        }
    }

    private int _deckIndex = -1;
    private int _cardIndex = -1;

    protected void Start() {
        _animator = GetComponent<DialogueAnimator>();
    }

    public override void ShowNextCard() {
        if (Dialogue == null) {
            return;
        }
        
        if (Dialogue.Decks.Count <= _deckIndex) {
            SkipDialogue();
            return;
        }

        var currentDeck = Dialogue.Decks[_deckIndex];
        _animator.ChangeSpeaker(currentDeck.Speaker);

        if (currentDeck.Cards.Count <= _cardIndex) {
            _deckIndex++;
            _cardIndex = 0;
            ShowNextCard();
        } else {
            var nextCard = currentDeck.Cards[_cardIndex];
            _cardIndex++;

            _animator.AnimateCard(nextCard);
        }
    }

    public override void SkipToEndOfCard() {
        throw new NotImplementedException();
    }
}
