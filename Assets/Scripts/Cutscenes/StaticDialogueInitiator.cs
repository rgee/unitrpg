using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StaticDialogueInitiator : MonoBehaviour {
    public TextAsset DialogueJson;
    public AbstractDialogueController DialogueController;

    void Start() {
        var cutscene = Models.Dialogue.DialogueUtils.ParseFromJson(DialogueJson.text);
        DialogueController.Dialogue = cutscene;
        DialogueController.ShowNextCard();
    }
}
