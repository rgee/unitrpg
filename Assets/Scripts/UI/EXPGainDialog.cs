using System;
using UnityEngine;
using UnityEngine.UI;

public class EXPGainDialog : MonoBehaviour {
    private EXPBubble Bubble;
    public int CharacterLevel;
    public string CharacterName;
    private Text EXPObject;
    private Text LevelObject;
    private Text NameObject;
    public int StartingEXP;

    private void Start() {
        Bubble = transform.Find("Panel/EXP Bubble").GetComponent<EXPBubble>();
        NameObject = transform.Find("Panel/Character Name").GetComponent<Text>();
        LevelObject = transform.Find("Panel/Character Level").GetComponent<Text>();
        EXPObject = transform.Find("Panel/EXP").GetComponent<Text>();
    }

    private void Update() {
        NameObject.text = CharacterName.ToUpper();
        LevelObject.text = "LEVEL " + StartingEXP;

        // Works as long as we level at 100 EXP every time.
        EXPObject.text = "XP " + Math.Floor(Bubble.ExpPercent) + " / 100";
    }
}