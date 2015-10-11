using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using UnityEngine;

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

    protected override void ChangeEmotion(EmotionType emotion) {
        _portraitView.SetActor(_currentSpeakerName, emotion);
    }

    protected override void ChangeSpeaker(string speakerName) {
        base.ChangeSpeaker(speakerName);
        _currentSpeakerName = speakerName;
    }

    public override void SkipDialogue() {
        StartCoroutine(FadeOut()); 
    }
}
