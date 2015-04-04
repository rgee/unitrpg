using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class UnitController : MonoBehaviour {

	private Action CurrentCallback;
	private List<Vector3> CurrentPath;
	private Vector3 PreviousPoint;
	private int CurrentPathIdx = -1;
	private Animator Animator;
	
	private static Dictionary<MathUtils.CardinalDirection, int> animatorDirections = new Dictionary<MathUtils.CardinalDirection, int>() {
		{ MathUtils.CardinalDirection.W, 1},
		{ MathUtils.CardinalDirection.N, 2},
		{ MathUtils.CardinalDirection.E, 3},
		{ MathUtils.CardinalDirection.S, 0}
	};

	public void Start() {
		Animator = GetComponent<Animator>();
	}

	public void MoveAlongPath(List<Vector3> path, Action callback) {
		CurrentPath = path;
		CurrentPathIdx = -1;
		CurrentCallback = callback;

		Animator.SetBool("Running", true);
		PreviousPoint = transform.position;

		StartNextSegment();
	}

	private void StartNextSegment() {
		CurrentPathIdx++;
		if (CurrentPathIdx > 0) {
			PreviousPoint = CurrentPath[CurrentPathIdx -1];
		}
		if (CurrentPathIdx < CurrentPath.Count) {
			Vector3 currentDestination = CurrentPath[CurrentPathIdx];
			MathUtils.CardinalDirection dir = MathUtils.DirectionTo(PreviousPoint, currentDestination);
			Animator.SetInteger("Direction", animatorDirections[dir]);

			iTween.MoveTo(gameObject, iTween.Hash(
				"position", currentDestination,
				"time", 0.3f,
				"oncomplete", "StartNextSegment",
				"easetype", iTween.EaseType.linear
			));
		} else {
			Animator.SetBool("Running", false);
			CurrentCallback();
		}
	}
}
