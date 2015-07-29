using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicSortedObject : MonoBehaviour {
    private float GridSize = 32;
    private Vector2 Offset = Vector2.zero;

    private Renderer _renderer;
    private GameObject _map;

    void Start() {
        _renderer = GetComponent<Renderer>();
    }

    void Update() {
        AlignToMap();
        var position = transform.position + new Vector3(Offset.x, Offset.y);
        _renderer.sortingOrder = -Mathf.RoundToInt(position.y/GridSize);
    }

    protected void AlignToMap() {
        if (_map == null) {
            var mapComponent = CombatObjects.GetMap();
            GridSize = mapComponent.tileSizeInPixels;
            _map = mapComponent.gameObject;
        }

        // First find a center for your bounds.
        var center = Vector3.zero;

        foreach (Transform child in _map.transform) {
            foreach (Transform childMesh in child) {
                var renderer = childMesh.GetComponent<Renderer>();
                if (renderer != null) {
                    center += renderer.bounds.center;
                }
            }
        }
        center /= _map.transform.childCount; //center is average center of children

        //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
        var bounds = new Bounds(center, Vector3.zero);

        foreach (Transform child in _map.transform) {
            foreach (Transform childMesh in child) {
                var childRenderer = childMesh.GetComponent<Renderer>();
                if (childRenderer != null) {
                    bounds.Encapsulate(childRenderer.bounds);
                }
            }
        }

        Offset = new Vector2(0, bounds.size.y - GridSize);
    }
}
