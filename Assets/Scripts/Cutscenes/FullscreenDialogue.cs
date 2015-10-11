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

    private string _activeSpeakerName;
    private readonly List<GameObject> _slots = new List<GameObject>(4); 

    private void CreateSpeakers() {
        var index = 0;
        foreach (var speaker in Dialogue.Speakers) {
            var slot = _slots[index];
            var portraitView = slot.GetComponent<DialoguePortraitView>();
            portraitView.SetActor(speaker, EmotionType.DEFAULT);
            index++;
        }
    }

    protected override void Awake() {
        base.Awake();
        _slots.Add(transform.FindChild("Slots/Slot 0").gameObject);
        _slots.Add(transform.FindChild("Slots/Slot 1").gameObject);
        _slots.Add(transform.FindChild("Slots/Slot 2").gameObject);
        _slots.Add(transform.FindChild("Slots/Slot 3").gameObject);
    }

    public override void SkipDialogue() {
        Application.LoadLevel(NextSceneName);
    }

    protected override void ChangeSpeaker(string speakerName) {
        base.ChangeSpeaker(speakerName);
        _activeSpeakerName = speakerName;

        var portraits = _slots.Select(s => s.GetComponent<DialoguePortraitView>());

        portraits.ToList().ForEach(p => {
            if (p.ActorName == speakerName) {
                p.Activate();
            } else {
                p.Deactivate();
            }
        });
    }

    protected override void ChangeEmotion(EmotionType emotion) {
        var slot = _slots.Select(s => s.GetComponent<DialoguePortraitView>())
            .Single(s => s.ActorName == _activeSpeakerName);
        slot.SetActor(_activeSpeakerName, emotion);
    }
}