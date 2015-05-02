using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimator : MonoBehaviour {
    public Card card;
    public bool complete;
    public Text textObject;

    public void Reset() {
        complete = false;
    }

    public Coroutine StartAnimation() {
        return StartCoroutine("Animate");
    }

    public void Skip() {
        StopCoroutine("Animate");

        if (textObject == null) {
            return;
        }

        textObject.text = "";
        foreach (var line in card.lines) {
            textObject.text += line.text.Trim() + " ";
        }

        complete = true;
    }

    private IEnumerator Animate() {
        var sentenceIndex = 0;
        foreach (var line in card.lines) {
            // Show the sentence character at a time
            foreach (var letter in line.text.Trim()) {
                textObject.text += letter;
                yield return new WaitForSeconds(0.02f);
            }

            // Enforce spaces between sentences
            textObject.text += " ";

            var delay = 0.4f;
            if (line.delayOverride > 0f) {
                delay = line.delayOverride;
            }


            yield return new WaitForSeconds(delay);
            sentenceIndex++;
        }
        complete = true;
    }
}