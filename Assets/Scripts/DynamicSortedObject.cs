using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicSortedObject : MonoBehaviour {
    public float GridSize;
    public Vector2 Offset = Vector2.zero;

    private Renderer _renderer;

    void Start() {
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector3 position = transform.position + new Vector3(Offset.x, Offset.y);
        _renderer.sortingOrder = -Mathf.RoundToInt(position.y/GridSize);
    }
}
