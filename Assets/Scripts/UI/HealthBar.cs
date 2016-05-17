using System;
using UnityEngine;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour {
    public float HealthPct = 100;
    private float _barWidth;
    private RectTransform _maskTransform;
    private RectTransform _barTransform;

    private void Awake() {
        _maskTransform = transform.FindChild("Mask").GetComponent<RectTransform>();
        _barTransform = transform.FindChild("Mask/Bar").GetComponent<RectTransform>();
    }

    private void Update() {
        var size = _maskTransform.sizeDelta;
        var barWidth = _barTransform.rect.width;
        var pct = Math.Min(HealthPct, 100);
        _maskTransform.sizeDelta = new Vector2((pct/100)*barWidth, size.y);
    }
}