using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class DialogueActor {
    public string Name;
    public List<DialoguePortrait> Portraits;

    public DialoguePortrait FindPortraitByEmotion(Models.EmotionType emotion) {
        foreach (var portrait in Portraits) {
            if (portrait.Emotion == emotion) {
                return portrait;
            }
        }

        throw new ArgumentException("Could not find portrait for emotion " + emotion);
    }
}
