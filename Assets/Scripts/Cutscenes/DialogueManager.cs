using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public TextAsset DialogueJson;
    public DialogueController DialogueController; 

    void Start() {
        var cutscene = Models.Dialogue.DialogueUtils.ParseFromJson(DialogueJson.text);
        DialogueController.Dialogue = cutscene;
        DialogueController.ShowNextCard();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DialogueController.ShowNextCard();
        }
    }
}
