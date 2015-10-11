using System.Collections;
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

    private readonly Dictionary<string, int> _slotIndexBySpeaker = new Dictionary<string, int>(); 

    private void CreateSpeakers() {

        var firstCard = Dialogue.Decks[0].Cards[0];
        foreach (var speaker in Dialogue.Speakers) {
            var firstEmotion = firstCard.EmotionalResponses[speaker];
            _slotIndexBySpeaker[speaker] = firstEmotion.Slot;
            var slot = _slots[firstEmotion.Slot];
            var portraitView = slot.GetComponent<DialoguePortraitView>();
            portraitView.SetActor(speaker, firstEmotion.emotion, firstEmotion.facing);
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
        var slotIndex = _slotIndexBySpeaker[speaker];
        if (slotIndex != response.Slot) {
            var currentSlot = _slots[slotIndex];
            var nextSlot = _slots[response.Slot];

            var currentView = currentSlot.GetComponent<DialoguePortraitView>();
            var nextView = nextSlot.GetComponent<DialoguePortraitView>();
            StartCoroutine(MoveActor(speaker, response, currentView, nextView));
        } else {
            var view = FindViewBySpeaker(speaker);
            view.SetActor(speaker, response.emotion, response.facing);
        }
    }

    private IEnumerator MoveActor(string speaker, EmotionalResponse response, DialoguePortraitView source,
        DialoguePortraitView destination) {

        yield return StartCoroutine(source.FadeToEmpty());
        yield return StartCoroutine(destination.FadeInActor(speaker, response.emotion, response.facing));
        _slotIndexBySpeaker[speaker] = response.Slot;
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