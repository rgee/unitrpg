using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class MovementDialog : MonoBehaviour {
	public GameObject PipPrefab;
	public int TotalMoves;
	public int UsedMoves;

	private List<GameObject> Pips = new List<GameObject>();

	void Start () {
		GameObject pipParent = transform.FindChild("Panel/Pips").gameObject;
		for (int i = 0; i < TotalMoves; i++) {
			GameObject pip = Instantiate(PipPrefab) as GameObject;
			Pips.Add(pip);

			pip.transform.parent = pipParent.transform;
		}
	}
	
	void Update () {
		int enabledPips = TotalMoves - UsedMoves;
		int numEnabled = 0;

		foreach (GameObject pip in Pips) {
			Pip comp = pip.GetComponent<Pip>();
			if (numEnabled < enabledPips) {
				comp.IsEnabled = true;
				numEnabled++;
			} else {
				comp.IsEnabled = false;
			}
		}
	}
}
