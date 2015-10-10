using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FullscreenTk2dLayout : MonoBehaviour {
    private tk2dUILayout _layout;
    void Start() {
        _layout = GetComponent<tk2dUILayout>();
    }

    void LateUpdate() {
        _layout.SetBounds(
            new Vector3(tk2dCamera.Instance.ScreenExtents.xMin, tk2dCamera.Instance.ScreenExtents.yMin, 0),
            new Vector3(tk2dCamera.Instance.ScreenExtents.xMax, tk2dCamera.Instance.ScreenExtents.yMax, 0)
        );
        
    }
}
