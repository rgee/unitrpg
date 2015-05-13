using Models.Combat.Inventory;
using UnityEditor;
using UnityEngine;

public class ItemEditor : MonoBehaviour {

    [MenuItem("Assets/Create/Item")]
    public static void CreateAsset() {
        CustomAssetCreator.CreateAsset<Item>();
    }
}
