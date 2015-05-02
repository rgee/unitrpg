using System;
using UnityEngine;

public class PhaseText : MonoBehaviour {
    private float Delay;
    private float MoveTime;
    private Vector2 OffScreen;
    private Action OnComplete;
    private RectTransform RectTransform;

    public void Awake() {
        RectTransform = GetComponent<RectTransform>();
    }

    public void MoveThroughScreen(PhaseTextFlyByCommand command, Action onComplete) {
        MoveTime = command.moveTime;
        Delay = command.pause;
        OnComplete = onComplete;
        OffScreen = command.offscreen;

        iTween.ValueTo(gameObject, iTween.Hash(
            "time", MoveTime,
            "from", RectTransform.anchoredPosition,
            "to", command.center,
            "onupdate", "SetNewPosition",
            "oncomplete", "MoveOffScreen"
            ));
    }

    private void SetNewPosition(Vector2 pos) {
        RectTransform.anchoredPosition = pos;
    }

    private void MoveOffScreen() {
        iTween.ValueTo(gameObject, iTween.Hash(
            "time", MoveTime,
            "delay", Delay,
            "from", RectTransform.anchoredPosition,
            "to", OffScreen,
            "onupdate", "SetNewPosition",
            "oncomplete", "OnMoveComplete"
            ));
    }

    private void OnMoveComplete() {
        OnComplete();
    }
}