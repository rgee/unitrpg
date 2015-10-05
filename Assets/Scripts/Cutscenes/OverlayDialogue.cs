using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class OverlayDialogue : AbstractDialogue {
    public DialogueActors Actors;
    public float FadeTime = .5f;

    private CanvasGroup _canvasGroup;
    private string _currentSpeakerName;
    private GameObject _portraitContainer;
    private GameObject _currentPortrait;

    protected override void Awake() {
        base.Awake(); 
        _canvasGroup = GetComponent<CanvasGroup>();
        _portraitContainer = transform.FindChild("Panel/Portrait").gameObject;
    }

    private IEnumerator FadeOut() {
        while (_canvasGroup.alpha > 0) {
            _canvasGroup.alpha -= Time.deltaTime / FadeTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    protected override void ChangeEmotion(EmotionType emotion) {
        Destroy(_currentPortrait);

        var actor = Actors.FindByName(_currentSpeakerName);
        var portrait = actor.FindPortraitByEmotion(emotion);

        _currentPortrait = Instantiate(portrait.Prefab);
        _currentPortrait.transform.SetParent(_portraitContainer.transform);
    }

    protected override void ChangeSpeaker(string speakerName) {
        base.ChangeSpeaker(speakerName);
        _currentSpeakerName = speakerName;
    }

    public override void SkipDialogue() {
        StartCoroutine(FadeOut()); 
    }
}
