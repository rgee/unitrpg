using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitController : MonoBehaviour {
    private static readonly Dictionary<MathUtils.CardinalDirection, int> animatorDirections =
        new Dictionary<MathUtils.CardinalDirection, int> {
            {MathUtils.CardinalDirection.W, 1},
            {MathUtils.CardinalDirection.N, 2},
            {MathUtils.CardinalDirection.E, 3},
            {MathUtils.CardinalDirection.S, 0}
        };

    private Animator Animator;
    private Action CurrentCallback;
    private List<Vector3> CurrentPath;
    private int CurrentPathIdx = -1;
    private Vector3 PreviousPoint;

    public void Start() {
        Animator = GetComponent<Animator>();
    }

    public void MoveAlongPath(List<Vector3> path, Action callback) {
        CurrentPath = path.Distinct().ToList();
        CurrentPathIdx = -1;
        CurrentCallback = callback;

        Animator.SetBool("Running", true);
        PreviousPoint = transform.position;

        StartNextSegment();
    }

    private void StartNextSegment() {
        CurrentPathIdx++;
        if (CurrentPathIdx > 0) {
            PreviousPoint = CurrentPath[CurrentPathIdx - 1];
        }
        if (CurrentPathIdx < CurrentPath.Count) {
            var currentDestination = CurrentPath[CurrentPathIdx];
            var dir = MathUtils.DirectionTo(PreviousPoint, currentDestination);
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