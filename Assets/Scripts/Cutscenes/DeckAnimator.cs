using System;
using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class DeckAnimator : MonoBehaviour {
    [HideInInspector] public CardAnimator animator;

    private int cardIdx;
    public bool complete;
    public Deck deck;
    public Text textObject;

    public void Reset() {
        complete = false;
        cardIdx = 0;
    }

    public void Update() {
        if (!complete) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                var currentTimeMs = DateTime.Now.Ticks;
                if (animator.complete) {
                    // Completing a card should wipe the textbox to make room for the next.
                    textObject.text = "";
                    StartAnimation();
                } else {
                    StartCoroutine(EndCurrentCard());
                }
            }
        }
    }

    /// <summary>
    ///     Begin animating the current deck.
    /// </summary>
    public void StartAnimation() {
        StartCoroutine(AnimateCurrentCard());
    }

    /// <summary>
    ///     Set up the state to look at the next card in the list.
    /// </summary>
    private void NextCard() {
        cardIdx++;
        if (cardIdx >= deck.cards.Length) {
            complete = true;
        }
    }

    private IEnumerator EndCurrentCard() {
        // We must wait for the end of this frame to do this, otherwise:
        // The 'Skip' call will force the card animator to complete, 
        // and if the CutsceneDirector's Update method runs after ours
        // it will see that in this frame, both the card animation is complete
        // and the space bar is down, so it will immediately advance to the next deck.
        //
        // This way, nothing in this frame will see that the card animator completed,
        // and on the next frame, the space key will probably have been released.
        yield return new WaitForEndOfFrame();
        animator.Skip();
        NextCard();
    }

    private IEnumerator AnimateCurrentCard() {
        Debug.Log("current");
        var currentCard = deck.cards[cardIdx];
        animator.textObject = textObject;
        animator.card = currentCard;
        animator.Reset();
        yield return animator.StartAnimation();

        NextCard();
    }
}