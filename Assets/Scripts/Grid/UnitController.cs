using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UnitController : MonoBehaviour {

	private List<Vector3> CurrentPath;
	private int CurrentPathIdx = -1;

	public void MoveAlongPath(List<Vector3> path) {
		CurrentPath = path;
		CurrentPathIdx = -1;

		StartNextSegment();
	}

	private void StartNextSegment() {
		CurrentPathIdx++;
		if (CurrentPathIdx < CurrentPath.Count) {
			iTween.MoveTo(gameObject, iTween.Hash(
				"position", CurrentPath[CurrentPathIdx],
				"time", 0.3f,
				"oncomplete", "StartNextSegment",
				"easetype", iTween.EaseType.linear
			));
		}
	}
}
