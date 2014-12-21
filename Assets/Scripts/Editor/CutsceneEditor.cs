using UnityEngine;
using UnityEditor;
using System.Collections;

public class CutsceneEditor : MonoBehaviour {

	[MenuItem("Assets/Create/Cutscene")]
	public static void CreateAsset()
	{
		CustomAssetCreator.CreateAsset<Models.Cutscene>();
	}
}
