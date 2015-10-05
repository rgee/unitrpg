using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using UnityEngine;

public class FullscreenDialogue : AbstractDialogue {
    public string NextSceneName;
    public DialogueActors Actors;

    public override Models.Dialogue.Cutscene Dialogue {
        get { return base.Dialogue; }
        set
        {
            base.Dialogue = value;
            CreateSpeakers();
        }
    }

    private GameObject _speakerContainer;
    private Dictionary<string, GameObject> _speakersByName = new Dictionary<string, GameObject>();
    private string _activeSpeakerName;
    private GameObject _activeSpeaker {
        get { return _speakersByName[_activeSpeakerName];  }
    }

    private void CreateSpeakers() {
        foreach (var speaker in Dialogue.Speakers) {
            var defaultPortrait = Actors.FindByName(speaker).FindPortraitByEmotion(EmotionType.DEFAULT);

            // TODO: Child ordering
            var speakerObject = Instantiate(defaultPortrait.Prefab);
            _speakersByName[speaker] = speakerObject;
            speakerObject.transform.SetParent(_speakerContainer.transform);
        }
    }

    protected override void Awake() {
        base.Awake();
        _speakerContainer = transform.FindChild("Speakers").gameObject;
    }

    public override void SkipDialogue() {
        Application.LoadLevel(NextSceneName);
    }

    protected override void ChangeSpeaker(string speakerName) {
        base.ChangeSpeaker(speakerName);
        _activeSpeakerName = speakerName;
    }

    protected override void ChangeEmotion(EmotionType emotion) {
        var currentSpeakerPortrait = _activeSpeaker;
        Destroy(currentSpeakerPortrait);

        var newPortrait = Actors.FindByName(_activeSpeakerName).FindPortraitByEmotion(emotion);
        
        // TODO: Child ordering
        var newPortraitObject = Instantiate(newPortrait.Prefab);
        newPortraitObject.transform.SetParent(_speakerContainer.transform);
        _speakersByName[_activeSpeakerName] = newPortraitObject;
    }
}