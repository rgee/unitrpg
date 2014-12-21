using UnityEngine;
using UnityEditor;
using System.Collections;

public class DeckEditor : MonoBehaviour {

	[MenuItem("Assets/Create/Deck")]
	public static void CreateAsset()
	{
		CustomAssetCreator.CreateAsset<Models.Deck>();
	}
}
