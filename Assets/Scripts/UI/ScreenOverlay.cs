using UnityEngine;

[RequireComponent(typeof(tk2dSprite))]
public class ScreenOverlay : MonoBehaviour {
    private tk2dSprite _sprite;

    private void Awake() {
        _sprite = GetComponent<tk2dSprite>();
    }

    public void FadeIn(float seconds) {
        iTween.ValueTo(gameObject, iTween.Hash(
            "time", seconds,
            "from", new Color(0, 0, 0, 0),
            "to", new Color(1, 1, 1, 1),
            "onupdate", "SetSpriteColor",
            "onupdatetarget", gameObject
        ));
    }

    public void FadeOut(float seconds) {
        iTween.ValueTo(gameObject, iTween.Hash(
            "time", seconds,
            "from", new Color(1, 1, 1, 1),
            "to", new Color(0, 0, 0, 0),
            "onupdate", "SetSpriteColor",
            "onupdatetarget", gameObject
        ));
    }

    private void SetSpriteColor(Color c) {
        _sprite.color = c;
    }
}