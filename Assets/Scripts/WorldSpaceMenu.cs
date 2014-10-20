using UnityEngine;
using System.Collections;

public class WorldSpaceMenu : MonoBehaviour {

	void Start () {
		TextMesh[] meshes = GetComponentsInChildren<TextMesh>();
		foreach (TextMesh mesh in meshes) {
			MeshRenderer renderer = mesh.GetComponent<MeshRenderer>();
			renderer.sortingLayerName = "Prop";
			renderer.sortingOrder = 6;
		}
	}
}
