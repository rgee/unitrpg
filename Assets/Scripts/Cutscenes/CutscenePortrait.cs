using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class CutscenePortrait : MonoBehaviour {
    private static readonly Color DEACTIVATED_COLOR = new Color(0.5f, 0.5f, 0.5f);
    private static readonly Color ACTIVATED_COLOR = Color.white;
    private static readonly float TINT_DURATION_SECONDS = 0.5f;
    private static readonly float EPSILON = 0.1f;
    private Image Body;
    public EmotionType currentEmotion = EmotionType.DEFAULT;
    public List<Emotion> Emotions;
    private Image Head;
    private Image Portrait;

    private void Start() {
        Portrait = GetComponent<Image>();
        Head = transform.FindChild("Head").gameObject.GetComponent<Image>();
        Head.sprite = FindSpriteForEmotion(EmotionType.DEFAULT);

        Body = transform.FindChild("Body").gameObject.GetComponent<Image>();
    }

    private void Update() {
        Head.sprite = FindSpriteForEmotion(currentEmotion);
    }

    public void ChangeEmotion(EmotionType type) {
        Head.sprite = FindSpriteForEmotion(type);
    }

    private Sprite FindSpriteForEmotion(EmotionType type) {
        foreach (var emo in Emotions) {
            if (emo.type == type) {
                return emo.face;
            }
        }

        return null;
    }

    private static bool ColorsAreSimilar(Color first, Color second) {
        var firstVec = new Vector3(first.r, first.b, first.g);
        var secondVec = new Vector3(second.r, second.b, second.g);
        return (firstVec - secondVec).magnitude < EPSILON;
    }

    public void Activate() {
        // Do not attempt to activate if already activated.
        // Use the similarity of colors as a heuristic because exact equality will likely fail by a small small
        // margin since we lerp the colors, and Unity doesn't guarantee to end on the actual color you provide. :(
        if (ColorsAreSimilar(Body.color, DEACTIVATED_COLOR) || ColorsAreSimilar(Head.color, DEACTIVATED_COLOR)) {
            StartCoroutine(TintToColor(DEACTIVATED_COLOR, ACTIVATED_COLOR, TINT_DURATION_SECONDS));
        }
    }

    public void Deactivate() {
        // Do not attempt to deactivate if already deactivated
        // Use the similarity of colors as a heuristic because exact equality will likely fail by a small small
        // margin since we lerp the colors, and Unity doesn't guarantee to end on the actual color you provide. :(
        if (ColorsAreSimilar(Body.color, ACTIVATED_COLOR) || ColorsAreSimilar(Head.color, ACTIVATED_COLOR)) {
            StartCoroutine(TintToColor(ACTIVATED_COLOR, DEACTIVATED_COLOR, TINT_DURATION_SECONDS));
        }
    }

    private IEnumerator TintToColor(Color startColor, Color color, float time) {
        float elapsedTime = 0;
        while (elapsedTime < time) {
            var nextColor = Color.Lerp(startColor, color, (elapsedTime/time));
            Body.color = nextColor;
            Head.color = nextColor;

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    [Serializable]
    public class Emotion {
        public Sprite face;
        public EmotionType type;
    }
}