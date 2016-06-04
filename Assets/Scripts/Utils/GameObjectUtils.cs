using UnityEngine;

namespace Utils {
    public static class GameObjectUtils {
        public static void SetLayerRecursively(GameObject root, int layer) {
            if (root == null) {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform) {
               SetLayerRecursively(child.gameObject, layer); 
            }
        }
    }
}