using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid.Unit))]
public class UnitEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Snap To Grid")) {
            var unitScript = (Grid.Unit) target;
            var transform = unitScript.transform;
            Undo.RecordObject(transform, "snap to grid");
            var map = CombatObjects.GetMap();
            transform.position = map.GetWorldPosForGridPos(unitScript.gridPosition);
            EditorUtility.SetDirty(transform);
        }
    }
}