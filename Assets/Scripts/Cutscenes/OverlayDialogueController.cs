using System.Collections;
using System.Collections.Generic;
using Models.Dialogue;
using UnityEngine;

public class OverlayDialogueController : MonoBehaviour, IDialogueController {
    private DialoguePortraitView _portraitView;
    private string _currentSpeakerName;
    private tk2dSlicedSprite _panelSprite;
    private tk2dTextMesh _bodyText;
    private tk2dTextMesh _speakerText;

    private const float IntroTime = 0.5f;

    private void Awake() {
        _portraitView = transform.FindChild("Portrait View").GetComponent<DialoguePortraitView>();
        _panelSprite = transform.FindChild("Text Panel").GetComponent<tk2dSlicedSprite>();
        _bodyText = transform.FindChild("Text Panel/Text").GetComponent<tk2dTextMesh>();
        _speakerText = transform.FindChild("Text Panel/Speaker").GetComponent<tk2dTextMesh>();
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
        var currentPosition = new Vector3(transform.position.x, transform.position.y*.5f, transform.position.z);
        var newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        var currentColor = new Color(_panelSprite.color.r, _panelSprite.color.g, _panelSprite.color.b, 0);
        var endColor = new Color(_panelSprite.color.r, _panelSprite.color.g, _panelSprite.color.b, _panelSprite.color.a);
        iTween.ValueTo(gameObject, iTween.Hash(
            "time", IntroTime,
            "from", currentColor,
            "to", endColor,
            "easetype", iTween.EaseType.easeInOutCubic,
            "onupdate", "SetPanelColor",
            "onupdatetarget", gameObject
        ));
        _portraitView.FadeIn(IntroTime);
        iTween.ValueTo(gameObject, iTween.Hash(
            "time", IntroTime,
            "from", currentPosition,
            "to", newPosition,
            "easetype", iTween.EaseType.easeInOutCubic,
            "onupdate", "SetPosition",
            "onupdatetarget", gameObject
        ));
        yield return new WaitForSeconds(IntroTime);
    }

    private void SetPanelColor(Color color) {
        _panelSprite.color = color;
    }

    private void SetPosition(Vector3 newPos) {
        transform.position = newPos;
    }

    private void SetTextColor(Color color) {
        _bodyText.color = color;
        _speakerText.color = color;
    }

    public IEnumerator End() {
        var currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        var newPosition = new Vector3(transform.position.x, transform.position.y*.5f, transform.position.z);

        var endColor = new Color(_panelSprite.color.r, _panelSprite.color.g, _panelSprite.color.b, 0);
        var currentColor = new Color(_panelSprite.color.r, _panelSprite.color.g, _panelSprite.color.b, _panelSprite.color.a);
        iTween.ValueTo(gameObject, iTween.Hash(
            "time", IntroTime,
            "from", currentColor,
            "to", endColor,
            "easetype", iTween.EaseType.easeInOutCubic,
            "onupdate", "SetPanelColor",
            "onupdatetarget", gameObject
        ));

        _portraitView.FadeOut(IntroTime);

        iTween.ValueTo(gameObject, iTween.Hash(
            "time", IntroTime,
            "to", new Color(1, 1, 1, 0),
            "from", new Color(1, 1, 1, 1),
            "easetype", iTween.EaseType.easeInOutCubic,
            "onupdate", "SetTextColor",
            "onupdatetarget", gameObject
        ));

        iTween.ValueTo(gameObject, iTween.Hash(
            "time", IntroTime,
            "from", currentPosition,
            "to", newPosition,
            "easetype", iTween.EaseType.easeInOutCubic,
            "onupdate", "SetPosition",
            "onupdatetarget", gameObject
        ));
        yield return new WaitForSeconds(IntroTime);
        Destroy(gameObject);
    }

}