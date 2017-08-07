using System.Collections;
using DG.Tweening;
using Models;
using UnityEngine;

[RequireComponent(typeof(IPortraitAligner))]
public class DialoguePortraitView : MonoBehaviour {
    public DialogueActors ActorDatabase;
    public float Scale = 0.0008f;

    [HideInInspector]
    public string ActorName;

    private GameObject _currentActorPortrait;
    private static readonly Color FadedColor = new Color(.3f, .3f, .3f);
    private const float FadeTimeSeconds = 1f;

    private IPortraitAligner _portraitAligner;
    private tk2dSprite _headSprite;
    private tk2dSprite _bodySprite;

    void Awake() {
        _portraitAligner = GetComponent<IPortraitAligner>();
    }

    public Tween GetFadeInTween(float time) {
        return DOTween.Sequence()
            .Append(_headSprite.DOFade(1, time))
            .Insert(0, _bodySprite.DOFade(1, time));
    }

    public Tween GetFadeOutTween(float time) {
        return DOTween.Sequence()
            .Append(_headSprite.DOFade(0, time))
            .Insert(0, _bodySprite.DOFade(0, time));
    }

    public void FadeOut(float time) {
        GetFadeOutTween(time).Play();
    }

    public void FadeIn(float time) {
        DOTween.Sequence()
            .Append(_headSprite.DOFade(1, time))
            .Insert(0, _bodySprite.DOFade(1, time))
            .Play();
    }

    public void SetActor(string name, EmotionType emotion, Facing direction) {
        var actor = ActorDatabase.FindByName(name);

        // If the line refers to an actor that doesn't exist, say "???", for example,
        // just show nothing.
        if (actor == null) {
           ClearGameObject(); 
        } else {
            var portrait = actor.FindPortraitByEmotion(emotion, direction);
            var portraitObject = Instantiate(portrait.Prefab);
            AttachGameObject(portraitObject);
            _portraitAligner.Align(portraitObject, direction, new Vector3(Scale, Scale, 1f));

            ActorName = name;
        }
    }

    private void ClearGameObject() {
        Destroy(_currentActorPortrait);
        _currentActorPortrait = null;
    }

    private void AttachGameObject(GameObject instantiatedGameObject) {
        if (_currentActorPortrait != null) {
            ClearGameObject();
        }

        instantiatedGameObject.transform.SetParent(transform);
        _currentActorPortrait = instantiatedGameObject;
        _headSprite = _currentActorPortrait.transform.FindChild("Head").GetComponent<tk2dSprite>();
        _bodySprite = _currentActorPortrait.transform.FindChild("Body").GetComponent<tk2dSprite>();
    }

    public IEnumerator FadeToEmpty() {
        if (_currentActorPortrait != null) {
            Destroy(_currentActorPortrait);
            ActorName = null;
        }

        yield return null;
    }

    public IEnumerator FadeInActor(string name, EmotionType emotion, Facing direction) {
        SetActor(name, emotion, direction);
        yield return null;
    }

    public void Activate() {
        if (_currentActorPortrait == null) {
            return;
        }

        FadeTo(Color.white, FadeTimeSeconds);
    }

    public void Deactivate() {
        if (_currentActorPortrait == null) {
            return;
        }

        FadeTo(FadedColor, FadeTimeSeconds);
    }

    private void FadeTo(Color color, float time) {
        DOTween.Sequence()
            .Append(_headSprite.DOColor(color, time))
            .Join(_bodySprite.DOColor(color, time))
            .Play();
    }
}