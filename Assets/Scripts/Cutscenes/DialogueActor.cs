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

        return null;
    }

    public DialoguePortrait FindPortraitByEmotion(Models.EmotionType emotion, Facing facing) {
        foreach (var portrait in Portraits) {
            if (portrait.Emotion == emotion && portrait.Facing == facing) {
                return portrait;
            }
        }
        return null;
    }
}
