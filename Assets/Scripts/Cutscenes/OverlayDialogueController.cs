using System.Collections;
using Models.Dialogue;
using UnityEngine;

public class OverlayDialogueController : MonoBehaviour, IDialogueController {
    private DialoguePortraitView _portraitView;
    private string _currentSpeakerName;

    private void Awake() {
        _portraitView = transform.FindChild("Portrait View").GetComponent<DialoguePortraitView>();
    }

    public void ChangeSpeaker(string speaker) {
        _currentSpeakerName = speaker;
    }

    public void ChangeEmotion(string speaker, EmotionalResponse response) {
        // The overlay only updates the actor currently speaking, and they always face left.
        if (speaker == _currentSpeakerName) {
            _portraitView.SetActor(speaker, response.emotion, Facing.Left);
        }
    }

    public IEnumerator Initialize(Cutscene model) {
        yield return null;
    }

    public IEnumerator End() {
        Destroy(gameObject);
        yield return null;
    }
}