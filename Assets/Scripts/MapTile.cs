using UnityEngine;
using System.Collections;

public class MapTile : MonoBehaviour {

    public GameObject occupant;

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform) {
            occupant = child.gameObject;
        }

	}
	
	// Update is called once per frame
	void Update () {
	}
}
