using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;

public class RadialMenuLayout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Vector2 ActivationOffset = new Vector2(10, 10);
    public float ActivationTimeSeconds = 0.3f;

    private readonly Dictionary<GameObject, Vector2> _restingPositions = new Dictionary<GameObject, Vector2>();

    public void Start() {
        foreach (Transform child in transform) {
            _restingPositions.Add(child.gameObject, ((RectTransform)child).anchoredPosition);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        var target = eventData.pointerCurrentRaycast.gameObject;
        if (target.transform.parent != transform) {
            return;
        }
        
        var rectTransform = (RectTransform) transform;
        var center = rectTransform.anchoredPosition;
        var targetCenter = _restingPositions[target];

        var destination = targetCenter - center;
        destination.Normalize();
        destination.Scale(ActivationOffset);

        iTween.ValueTo(target, iTween.Hash(
            "from", targetCenter,
            "to", targetCenter + destination,
            "time", ActivationTimeSeconds,
            "onupdate", "SetPosition",
            "easeType", iTween.EaseType.easeOutQuad
        ));
    }

    public void OnPointerExit(PointerEventData eventData) {
        var target = eventData.pointerEnter;
        if (target.transform.parent != transform) {
            return;
        }

        var targetCenter = _restingPositions[target];

        iTween.ValueTo(target, iTween.Hash(
            "from", ((RectTransform)target.transform).anchoredPosition,
            "to", targetCenter,
            "time", ActivationTimeSeconds,
            "onupdate", "SetPosition",
            "easeType", iTween.EaseType.easeOutQuad
        ));
    }
}