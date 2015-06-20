using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicSortedObject : MonoBehaviour {
    public float GridSize;

    private Renderer _renderer;

    void Start() {
        _renderer = GetComponent<Renderer>();
    }

    void Update() {
        _renderer.sortingOrder = -Mathf.RoundToInt(transform.position.y/GridSize);
    }
}
