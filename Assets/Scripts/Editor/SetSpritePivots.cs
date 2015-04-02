using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

class SetSpritePivots {
    [MenuItem("Sprite/Set Pivot(s)")]
    static void SetPivots() {

        UnityEngine.Object[] textures = GetSelectedTextures();

        Selection.objects = new UnityEngine.Object[0];
        foreach (Texture2D texture in textures) {
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            List<SpriteMetaData> newData = new List<SpriteMetaData>();
            for (int i = 0; i < ti.spritesheet.Length; i++) {
                SpriteMetaData d = ti.spritesheet[i];
                d.alignment = 9;
                d.pivot = ti.spritesheet[0].pivot;
                newData.Add(d);
            }
            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }

    static UnityEngine.Object[] GetSelectedTextures() {
        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    }
}
