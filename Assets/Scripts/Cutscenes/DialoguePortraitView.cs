using System.Collections;
using Models;
using UnityEngine;

public class DialoguePortraitView : MonoBehaviour {
    public DialogueActors ActorDatabase;
    public string ActorName;

    private GameObject _currentActorPortrait;
    private static readonly Color FADED_COLOR = new Color(92f/255f, 92f/255f, 92f/255f);
    private static readonly float FADE_TIME_SECONDS = 1f;

    public void SetActor(string name, EmotionType emotion) {
        var actor = ActorDatabase.FindByName(name);
        var portrait = actor.FindPortraitByEmotion(emotion);
        AttachGameObject(Instantiate(portrait.Prefab));
        ActorName = name;
    }

    private void AttachGameObject(GameObject instantiatedGameObject) {
        if (_currentActorPortrait != null) {
            Destroy(_currentActorPortrait);
        }

        instantiatedGameObject.transform.SetParent(transform);
        instantiatedGameObject.transform.localScale = new Vector3(0.0005f, 0.0005f, 1f);
        instantiatedGameObject.transform.localPosition = new Vector3(0.341f, -0.522f, -0.9f);
        _currentActorPortrait = instantiatedGameObject;
    }

    public void Activate() {
        if (_currentActorPortrait == null) {
            return;
        }

        StartCoroutine(FadeTo(Color.white, FADE_TIME_SECONDS));
    }

    public void Deactivate() {
        if (_currentActorPortrait == null) {
            return;
        }

        StartCoroutine(FadeTo(FADED_COLOR, FADE_TIME_SECONDS));
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