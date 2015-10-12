using System.Collections;
using Models;
using UnityEngine;

[RequireComponent(typeof(IPortraitAligner))]
public class DialoguePortraitView : MonoBehaviour {
    public DialogueActors ActorDatabase;
    public float Scale = 0.0008f;

    [HideInInspector]
    public string ActorName;

    private GameObject _currentActorPortrait;
    private static readonly Color FadedColor = new Color(92f/255f, 92f/255f, 92f/255f);
    private const float FadeTimeSeconds = 1f;

    private IPortraitAligner _portraitAligner;

    void Awake() {
        _portraitAligner = GetComponent<IPortraitAligner>();
    }

    public void FadeOut(float time) {
       iTween.ValueTo(gameObject, iTween.Hash(
            "time", time,
            "from", new Color(1, 1, 1, 1),
            "to", new Color(1, 1, 1, 0),
            "easetype", iTween.EaseType.easeInOutCubic,
            "onupdate", "SetColor",
            "onupdatetarget", gameObject
       ));
    }

    public void FadeIn(float time) {
       iTween.ValueTo(gameObject, iTween.Hash(
            "time", time,
            "from", new Color(1, 1, 1, 0),
            "to", new Color(1, 1, 1, 1),
            "easetype", iTween.EaseType.easeInOutCubic,
            "onupdate", "SetColor",
            "onupdatetarget", gameObject
       ));
    }

    private void SetColor(Color color) {
        if (_currentActorPortrait == null) {
            return;
        }

        _currentActorPortrait.transform.FindChild("Head").GetComponent<tk2dSprite>().color = color;
        _currentActorPortrait.transform.FindChild("Body").GetComponent<tk2dSprite>().color = color;
    }

    public void SetActor(string name, EmotionType emotion, Facing direction) {
        var actor = ActorDatabase.FindByName(name);
        var portrait = actor.FindPortraitByEmotion(emotion);
        var portraitObject = Instantiate(portrait.Prefab);
        AttachGameObject(portraitObject);
        _portraitAligner.Align(portraitObject, direction, new Vector3(Scale, Scale, 1f));

        ActorName = name;
    }

    private void AttachGameObject(GameObject instantiatedGameObject) {
        if (_currentActorPortrait != null) {
            Destroy(_currentActorPortrait);
        }

        instantiatedGameObject.transform.SetParent(transform);
        _currentActorPortrait = instantiatedGameObject;
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

        StartCoroutine(FadeTo(Color.white, FadeTimeSeconds));
    }

    public void Deactivate() {
        if (_currentActorPortrait == null) {
            return;
        }

        StartCoroutine(FadeTo(FadedColor, FadeTimeSeconds));
    }

    private IEnumerator FadeTo(Color color, float time) {
        var head = _currentActorPortrait.transform.FindChild("Head").GetComponent<tk2dSprite>();
        var body = _currentActorPortrait.transform.FindChild("Body").GetComponent<tk2dSprite>();

        var headFromColor = head.color;
        var bodyFromColor = body.color;

        for (float t = 0; t < time; t += tk2dUITime.deltaTime) {
            var headColor = Color.Lerp(headFromColor, color, t);
            head.color = headColor;

            var bodyColor = Color.Lerp(bodyFromColor, color, t);
            body.color = bodyColor;

            yield return null;
        }
    }
}