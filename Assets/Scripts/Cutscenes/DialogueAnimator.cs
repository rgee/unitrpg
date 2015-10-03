using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAnimator : MonoBehaviour {
    private Text _speakerNameText;
    private Text _bodyText;

    void Start() {
        _speakerNameText = transform.FindChild("Panel/Text/Speaker Name").GetComponent<Text>();
        _bodyText = transform.FindChild("Panel/Text/Body").GetComponent<Text>();
    }

    public void ChangeSpeaker(string speakerName) {
        _speakerNameText.text = speakerName;
    }

    public void AnimateCard(Models.Dialogue.Card card) {
        var lines = string.Join(" ", card.Lines.ToArray());
        _bodyText.text = lines;
    }

    bool IsCardComplete() {
        return true;
    }
}