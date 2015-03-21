using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EXPGainDialog : MonoBehaviour {

	public string CharacterName;
	public int CharacterLevel;
	public int StartingEXP;

	private EXPBubble Bubble;
	private Text NameObject;
	private Text LevelObject;
	private Text EXPObject;

	void Start () {
		Bubble = transform.Find("Panel/EXP Bubble").GetComponent<EXPBubble>();
		NameObject = transform.Find("Panel/Character Name").GetComponent<Text>();
		LevelObject = transform.Find("Panel/Character Level").GetComponent<Text>();
		EXPObject = transform.Find("Panel/EXP").GetComponent<Text>();
	}
	
	void Update () {
		NameObject.text = CharacterName.ToUpper();
		LevelObject.text = "LEVEL " + StartingEXP.ToString();

		// Works as long as we level at 100 EXP every time.
		EXPObject.text = "XP " + Math.Floor(Bubble.ExpPercent).ToString() + " / 100";
	}
}
