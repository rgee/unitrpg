using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class OverlayDialogueController : AbstractDialogueController {
    private CanvasGroup _canvasGroup;
    public float FadeTime = .5f;

    void Start() {
        base.Start();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private IEnumerator FadeOut() {
        while (_canvasGroup.alpha > 0) {
            _canvasGroup.alpha -= Time.deltaTime / FadeTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    public override void SkipDialogue() {
        StartCoroutine(FadeOut()); 
    }
}
