using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelUpStat))]
public class LevelUpStatEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        LevelUpStat statScript = (LevelUpStat)target;
        if (GUILayout.Button("Increase")) {
            statScript.Increase();
        }
    }
}
