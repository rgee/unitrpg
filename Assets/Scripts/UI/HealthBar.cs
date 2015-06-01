﻿using UnityEngine;

public class HealthBar : MonoBehaviour {
    public float healthPct = 100;
    private float initialX;
    private RectTransform maskTransform;

    private void Start() {
        maskTransform = transform.FindChild("Mask").GetComponent<RectTransform>();
        initialX = maskTransform.sizeDelta.x;
    }

    private void Update() {
        var size = maskTransform.sizeDelta;
        maskTransform.sizeDelta = new Vector2((healthPct/100)*initialX, size.y);
    }
}