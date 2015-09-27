using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public TextAsset dialogueJson;

    public void Start() {
        var cutscene = Models.Dialogue.DialogueUtils.ParseFromJson(dialogueJson.text);
        Debug.Log(cutscene);
    }
}
