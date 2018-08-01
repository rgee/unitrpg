using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextAnimator : MonoBehaviour {
    private tk2dTextMesh _speakerNameText;
    private tk2dTextMesh _bodyText;
    private bool _animating;

    private static Dictionary<char, float> DELAY_BY_CHARACTER = new Dictionary<char, float>() {
        { ',', 0.1f },
        { ';', 0.1f },
        { '.', 0.2f },
        { ':', 0.1f }
    };

    private struct DelayedText {
        public readonly char Letter;
        public readonly float Delay;

        public DelayedText(char letter) {
            Letter = letter;

            if (DELAY_BY_CHARACTER.ContainsKey(letter)) { 
                Delay = DELAY_BY_CHARACTER[Letter];
            } else {
                Delay = 0f;
            }
        }
    }

    void Awake() {
        _speakerNameText = transform.Find("Text Panel/Speaker").GetComponent<tk2dTextMesh>();
        _bodyText = transform.Find("Text Panel/Text").GetComponent<tk2dTextMesh>();
    }

    public void ChangeSpeaker(string speakerName) {
        _speakerNameText.text = speakerName.ToUpper();
    }

    public void AnimateCard(Models.Dialogue.Card card) {
        StopAllCoroutines();
        var bodyCharacters = string.Join(" ", card.Lines.ToArray()).ToCharArray();
        var delayedCharcters = bodyCharacters.Select(c => new DelayedText(c));
        _bodyText.text = "";
        StartCoroutine(RenderDelayedCharacters(delayedCharcters));
    }

    private IEnumerator RenderDelayedCharacters(IEnumerable<DelayedText> characters) {
        foreach (var character in characters) {
            _bodyText.text += character.Letter;
            yield return new WaitForSeconds(character.Delay);
        }
    }
}
