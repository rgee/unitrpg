using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Grid.Unit))]
public class UnitController : MonoBehaviour {
    private static readonly Dictionary<MathUtils.CardinalDirection, int> animatorDirections =
        new Dictionary<MathUtils.CardinalDirection, int> {
            {MathUtils.CardinalDirection.W, 1},
            {MathUtils.CardinalDirection.N, 2},
            {MathUtils.CardinalDirection.E, 3},
            {MathUtils.CardinalDirection.S, 0}
        };

    private Action CurrentCallback;
    private List<Vector3> CurrentPath;
    private int CurrentPathIdx = -1;
    private Vector3 PreviousPoint;
    private BattleState _battleState;
    private Grid.Unit _unit;

    public void Start() {
        _unit = GetComponent<Grid.Unit>();
        _battleState = CombatObjects.GetBattleState();
    }

    public void MoveAlongPath(List<Vector3> path, Action callback) {
        CurrentPath = path.Distinct().ToList();
        CurrentPathIdx = -1;
        CurrentCallback = callback;

        _unit.Running = true;
        PreviousPoint = transform.position;
        StartNextSegment();
    }

    private void StartNextSegment() {
        CurrentPathIdx++;
        if (CurrentPathIdx > 0) {
            PreviousPoint = CurrentPath[CurrentPathIdx - 1];
        }
        if (CurrentPathIdx < CurrentPath.Count) {
            var currentDestination = MathUtils.Round(CurrentPath[CurrentPathIdx]);
            _unit.Facing = MathUtils.DirectionTo(MathUtils.Round(PreviousPoint), currentDestination);

            iTween.MoveTo(gameObject, iTween.Hash(
                "position", currentDestination,
                "time", 0.3f,
                "oncomplete", "StartNextSegment",
                "easetype", iTween.EaseType.linear
                ));
        } else {
            _unit.Running = false;
            CommitMoveToModel();
            CurrentCallback();
        }
    }

    private void CommitMoveToModel() {
        var unitModel = GetComponent<Grid.Unit>().model;
        var battleModel = _battleState.Model;
        var grid = CombatObjects.GetMap();

        var gridPointPath = (from point in CurrentPath
                             select grid.GridPositionForWorldPosition(point))
                             .ToList();

        var destination = gridPointPath.Last();

        battleModel.MoveUnit(unitModel, gridPointPath, destination);
    }
}