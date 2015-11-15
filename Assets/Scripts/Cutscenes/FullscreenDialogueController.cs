using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.Dialogue;
using UnityEngine;

public class FullscreenDialogueController : MonoBehaviour, IDialogueController {

    public string NextSceneName;
    private Cutscene _model;
    private string _activeSpeakerName;
    private readonly List<GameObject> _slots = new List<GameObject>(4); 

    private readonly Dictionary<string, int> _slotIndexBySpeaker = new Dictionary<string, int>(); 

    private void CreateSpeakers() {

        var firstCard = _model.Decks[0].Cards[0];
        foreach (var speaker in _model.Speakers) {
            var firstEmotion = firstCard.EmotionalResponses[speaker];
            _slotIndexBySpeaker[speaker] = firstEmotion.Slot;
            var slot = _slots[firstEmotion.Slot];
            var portraitView = slot.GetComponent<DialoguePortraitView>();
            portraitView.SetActor(speaker, firstEmotion.emotion, firstEmotion.facing);
        }
    }

    private void Awake() {
        _slots.Add(transform.FindChild("Slots/Slot 0").gameObject);
        _slots.Add(transform.FindChild("Slots/Slot 1").gameObject);
        _slots.Add(transform.FindChild("Slots/Slot 2").gameObject);
        _slots.Add(transform.FindChild("Slots/Slot 3").gameObject);
    }

    public void ChangeSpeaker(string speaker) {
        var currentSpeakerView = FindViewBySpeaker(_activeSpeakerName);
        if (currentSpeakerView) {
            currentSpeakerView.Deactivate();
        }

        FindViewBySpeaker(speaker).Activate();

        _activeSpeakerName = speaker;
    }

    public void ChangeEmotion(string speaker, EmotionalResponse response) {
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
    public IEnumerator Initialize(Cutscene model) {
        _model = model;
        CreateSpeakers();
        yield return null;
    }

    public IEnumerator End() {
        yield return null;
    }

    private IEnumerator MoveActor(string speaker, EmotionalResponse response, DialoguePortraitView source,
        DialoguePortraitView destination) {

        yield return StartCoroutine(source.FadeToEmpty());
        yield return StartCoroutine(destination.FadeInActor(speaker, response.emotion, response.facing));
        _slotIndexBySpeaker[speaker] = response.Slot;
    }

    private DialoguePortraitView FindViewBySpeaker(string name) {
        return _slots.Select(s => s.GetComponent<DialoguePortraitView>())
            .SingleOrDefault(s => s.ActorName == name);
    }

}
