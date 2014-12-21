using UnityEngine;
using UnityEditor;
using System.Collections;

public class CardEditor : MonoBehaviour {

	[MenuItem("Assets/Create/Card")]
	public static void CreateAsset()
	{
		CustomAssetCreator.CreateAsset<Models.Card>();
	}
}
