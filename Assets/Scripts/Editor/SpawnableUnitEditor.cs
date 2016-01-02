using Combat.ScriptedEvents;
using UnityEditor;
using UnityEngine;

public class SpawnableUnitEditor : MonoBehaviour {

    [MenuItem("Assets/Create/SpawnableUnit")]
    public static void CreateAsset() {
        CustomAssetCreator.CreateAsset<SpawnableUnit>();
    }
}
