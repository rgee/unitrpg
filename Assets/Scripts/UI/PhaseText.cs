using System;
using DG.Tweening;
using UnityEngine;

public class PhaseText : MonoBehaviour {
    private RectTransform _rectTransform;

    public void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void MoveThroughScreen(PhaseTextFlyByCommand command, Action onComplete) {
        var tweenSeq = DOTween.Sequence()
            .Append(_rectTransform.DOAnchorPos(command.center, command.moveTime).SetEase(Ease.OutCubic))
            .AppendInterval(command.pause)
            .Append(_rectTransform.DOAnchorPos(command.offscreen, command.moveTime).SetEase(Ease.InCubic))
            .OnComplete(() => onComplete());

        tweenSeq.Play();
    }
}