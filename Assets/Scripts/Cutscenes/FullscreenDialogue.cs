using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Dialogue;
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
            portraitView.SetActor(speaker, EmotionType.DEFAULT, Facing.Left);
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

    protected override void ChangeEmotion(string speaker, EmotionalResponse response) {
        var view = FindViewBySpeaker(speaker);
        view.SetActor(speaker, response.emotion, response.facing);
    }

    public override void SkipDialogue() {
        Application.LoadLevel(NextSceneName);
    }

    protected override void ChangeSpeaker(string speakerName) {
        base.ChangeSpeaker(speakerName);

        var currentSpeakerView = FindViewBySpeaker(_activeSpeakerName);
        if (currentSpeakerView) {
            currentSpeakerView.Deactivate();
        }

        FindViewBySpeaker(speakerName).Activate();

        _activeSpeakerName = speakerName;
    }

    private DialoguePortraitView FindViewBySpeaker(string name) {
        return _slots.Select(s => s.GetComponent<DialoguePortraitView>())
            .SingleOrDefault(s => s.ActorName == name);
    }
}