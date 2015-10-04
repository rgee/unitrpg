using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StaticDialogueInitiator : MonoBehaviour {
    public TextAsset SourceFile;
    public AbstractDialogue Dialogue;

    void Start() {
        var cutscene = Models.Dialogue.DialogueUtils.ParseFromJson(SourceFile.text);
        Dialogue.Dialogue = cutscene;
        Dialogue.ShowNextCard();
    }
}
