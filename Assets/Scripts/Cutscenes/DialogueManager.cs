using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public TextAsset DialogueJson;
    public DialogueActors Actors;

    public void Start() {
        var cutscene = Models.Dialogue.DialogueUtils.ParseFromJson(DialogueJson.text);
        Debug.Log(cutscene);

        var firstDeck = cutscene.Decks[0];
        var firstActor = Actors.FindByName(firstDeck.Speaker);

        var portrait = firstActor.FindPortraitByEmotion(Models.EmotionType.DEFAULT);
        Instantiate(portrait.Prefab);
    }
}
