using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveGames;
using UnityEngine;
using UnityEngine.UI;

public class SaveGameBubble : MonoBehaviour {
    public State State;

    void Start() {
        transform.FindChild("Chapter Name").GetComponent<Text>().text = "The Battle of Halhithe Square";

        var dateString = string.Format("{0:dddd, MMMM d, yyyy}", State.SavedOn);
        transform.FindChild("Saved On").GetComponent<Text>().text = dateString;
    }
}
