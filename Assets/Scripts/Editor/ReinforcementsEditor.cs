using Combat;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor {
    public class ReinforcementsEditor : MonoBehaviour {

        [MenuItem("Assets/Create/Reinforcements")]
        public static void CreateAsset() {
            CustomAssetCreator.CreateAsset<Reinforcements>();
        } 
    }
}