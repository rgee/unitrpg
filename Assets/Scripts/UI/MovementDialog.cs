using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MovementDialog : MonoBehaviour {
	public GameObject PipPrefab;
	public int TotalMoves;
	public int UsedMoves;

	private List<GameObject> Pips = new List<GameObject>();

	// Use this for initialization
	void Start () {
		GameObject pipParent = transform.FindChild("Panel/Pips").gameObject;
		for (int i = 0; i < TotalMoves; i++) {
			GameObject pip = Instantiate(PipPrefab) as GameObject;
			Pips.Add(pip);

			pip.transform.parent = pipParent.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
