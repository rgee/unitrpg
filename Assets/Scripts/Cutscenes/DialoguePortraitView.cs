using System.Collections;
using Models;
using UnityEngine;

public class DialoguePortraitView : MonoBehaviour {
    public DialogueActors ActorDatabase;
    public string ActorName;

    private GameObject _currentActorPortrait;
    private static readonly Color FadedColor = new Color(92f/255f, 92f/255f, 92f/255f);
    private const float FadeTimeSeconds = 1f;
    private const float Scale = 0.0008f;

    public void SetActor(string name, EmotionType emotion, Facing direction) {
        var actor = ActorDatabase.FindByName(name);
        var portrait = actor.FindPortraitByEmotion(emotion);
        AttachGameObject(Instantiate(portrait.Prefab), direction);
        ActorName = name;
    }

    private void AttachGameObject(GameObject instantiatedGameObject, Facing direction) {
        if (_currentActorPortrait != null) {
            Destroy(_currentActorPortrait);
        }
        var xScale = direction == Facing.Left ? Scale : -Scale;
        instantiatedGameObject.transform.localScale = new Vector3(xScale, Scale, 1f);

        var layout = GetComponent<tk2dUILayout>();
        var height = (layout.GetMinBounds() - layout.GetMaxBounds()).y;
        var halfLayoutWidth = (layout.GetMinBounds() - layout.GetMaxBounds()).x/2;
        var bounds = GameObjectUtils.CalculateBounds(instantiatedGameObject);
        var halfPortraitWidth = (bounds.max - bounds.min).x/2;

        var viewBottomCenter = -halfLayoutWidth;
        var portraitBottomCenter = direction == Facing.Left ? -halfPortraitWidth : halfPortraitWidth; 


        instantiatedGameObject.transform.SetParent(transform);
        instantiatedGameObject.transform.localPosition = new Vector3(viewBottomCenter + portraitBottomCenter, height, 0);
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