using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class DynamicSortedObject : MonoBehaviour {
    private float GridSize = 32;
    private Vector2 Offset = Vector2.zero;

    private Renderer _renderer;
    private GameObject _map;

    void Awake() {
        _renderer = GetComponent<Renderer>();
        UpdateSortOrder();
    }

    void Update() {
        UpdateSortOrder();
    }

    void UpdateSortOrder() {
        var position = transform.position + new Vector3(Offset.x, Offset.y);
        _renderer.sortingOrder = -Mathf.RoundToInt(position.y/GridSize);
    }
}
