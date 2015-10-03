using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class DialogueController : MonoBehaviour {
    public virtual Models.Dialogue.Cutscene Dialogue {
        get {
            return _dialogue;
        }

        set {
            _dialogue = value;
        }
    }

    private Models.Dialogue.Cutscene _dialogue;

    public abstract void SkipToEndOfCard();
    public abstract void ShowNextCard();
    public abstract void SkipDialogue();
}
