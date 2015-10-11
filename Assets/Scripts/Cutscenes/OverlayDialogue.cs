using System.Collections;
using Models.Dialogue;

public class OverlayDialogue : AbstractDialogue {
    public DialogueActors Actors;
    public float FadeTime = .5f;
    private DialoguePortraitView _portraitView;

    private string _currentSpeakerName;

    protected override void Awake() {
        base.Awake();
        _portraitView = transform.FindChild("Portrait View").GetComponent<DialoguePortraitView>();
    }

    private IEnumerator FadeOut() {
        Destroy(gameObject);
        yield return null;
    }

    protected override void ChangeEmotion(string speaker, EmotionalResponse response) {
        // The overlay only updates the actor currently speaking, and they always face left.
        if (speaker == _currentSpeakerName) {
            _portraitView.SetActor(speaker, response.emotion, Facing.Left);
        }
    }

    protected override void ChangeSpeaker(string speakerName) {
        base.ChangeSpeaker(speakerName);
        _currentSpeakerName = speakerName;
    }

    public override void SkipDialogue() {
        StartCoroutine(FadeOut()); 
    }
}
