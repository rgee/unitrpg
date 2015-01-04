using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGrid))]
public class MapGridEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        MapGrid grid = (MapGrid)target;
        if (GUILayout.Button("Rebuild Grid")) {
            grid.ResetTiles();
        }
    }
}
