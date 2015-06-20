using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicSortedObject : MonoBehaviour {
    public float GridSize = 32;
    public Vector2 Offset = Vector2.zero;

    private Renderer _renderer;

    void Start() {
        _renderer = GetComponent<Renderer>();
        var map = CombatObjects.GetMap();
        if (map != null) {
            Offset = new Vector2(0, map.transform.position.y);
        }
    }

    void Update() {
        Vector3 position = transform.position + new Vector3(Offset.x, Offset.y);
        _renderer.sortingOrder = -Mathf.RoundToInt(position.y/GridSize);
    }
}
