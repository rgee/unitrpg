using Combat.Props;
using UnityEditor;
using UnityEngine;

namespace Scripts.Editor {
    [CustomEditor(typeof(Chapter2House))]
    public class Chapter2HouseEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            var script = (Chapter2House) target;
            if (GUILayout.Button("Disable")) {
                script.StartCoroutine(script.Disable());
            }

            if (GUILayout.Button("Enable")) {
                script.StartCoroutine(script.Enable());
            }
        }
    }
}