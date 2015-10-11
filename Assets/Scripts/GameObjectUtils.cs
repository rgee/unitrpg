using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameObjectUtils {
    public static Bounds CalculateBounds(GameObject parent) {
        var renderer = parent.GetComponent<Renderer>();
        if (renderer != null) {
            var bounds = renderer.bounds;
            var renderers = parent.GetComponentsInChildren<Renderer>();
            foreach (var childRenderer in renderers.Where(childRenderer => renderer != childRenderer)) {
                bounds.Encapsulate(childRenderer.bounds);
            }

            return bounds;
        } else {
            var center = Vector3.zero;
            foreach (Transform childTransform in parent.transform) {
                center += childTransform.gameObject.GetComponent<Renderer>().bounds.center;
            }
            center /= parent.transform.childCount;
                
            var bounds = new Bounds(center, Vector3.zero);
            foreach (Transform childTransform in parent.transform) {
                bounds.Encapsulate(childTransform.GetComponent<Renderer>().bounds);
            }

            return bounds;
        }
    }
}