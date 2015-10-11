using System.Collections;
using Models.Dialogue;

interface IDialogueController {
    void ChangeSpeaker(string speaker);

    void ChangeEmotion(string speaker, EmotionalResponse response);

    IEnumerator Initialize(Cutscene model);

    IEnumerator End();
}