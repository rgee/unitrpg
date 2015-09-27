using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

public class DialogueActorsEditor : MonoBehaviour{
    [MenuItem("Assets/Create/DialogueActors")]
    public static void CreateAsset() {
        CustomAssetCreator.CreateAsset<DialogueActors>();
    }
}