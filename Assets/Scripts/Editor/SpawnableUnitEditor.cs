using Models.Combat.Inventory;
using UnityEditor;
using UnityEngine;

public class SpawnableUnitEditor : MonoBehaviour {

    [MenuItem("Assets/Create/SpawnableUnit")]
    public static void CreateAsset() {
        CustomAssetCreator.CreateAsset<ScriptedEvents.SpawnableUnit>();
    }
}
