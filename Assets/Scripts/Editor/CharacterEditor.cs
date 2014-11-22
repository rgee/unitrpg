using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterEditor : MonoBehaviour {

	[MenuItem("Assets/Create/Character")]
	public static void CreateAsset()
	{
		CustomAssetCreator.CreateAsset<Models.Character>();
	}
}
