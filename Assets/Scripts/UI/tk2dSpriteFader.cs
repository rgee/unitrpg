using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(tk2dSprite))]
public class tk2dSpriteFader : MonoBehaviour {
    private tk2dSprite _sprite;

    private void Awake() {
        _sprite = GetComponent<tk2dSprite>();
    }

    public IEnumerator FadeIn(float seconds) {
        yield return _sprite.DOFade(1f, seconds).WaitForCompletion();
    }

    public IEnumerator FadeOut(float seconds) {
        yield return _sprite.DOFade(0f, seconds).WaitForCompletion();
    }
}