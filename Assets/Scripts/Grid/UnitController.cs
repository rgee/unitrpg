using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Grid.Unit))]
public class UnitController : MonoBehaviour {
    private Action _currentCallback;
    private List<Vector3> _currentPath;
    private int _currentPathIdx = -1;
    private Vector3 _previousPoint;
    private BattleState _battleState;
    private Grid.Unit _unit;

    public void Start() {
        _unit = GetComponent<Grid.Unit>();
        _battleState = CombatObjects.GetBattleState();
    }

    public void MoveAlongPath(List<Vector3> path, Action callback) {
        _currentPath = path.Distinct().ToList();
        _currentPathIdx = -1;
        _currentCallback = callback;

        _unit.Running = true;
        _previousPoint = transform.position;
        StartNextSegment();
    }

    private void StartNextSegment() {
        _currentPathIdx++;
        if (_currentPathIdx > 0) {
            _previousPoint = _currentPath[_currentPathIdx - 1];
        }
        if (_currentPathIdx < _currentPath.Count) {
            var currentDestination = MathUtils.Round(_currentPath[_currentPathIdx]);
            _unit.Facing = MathUtils.DirectionTo(MathUtils.Round(_previousPoint), currentDestination);

            var secondsPerSquare = _unit.model.Character.MoveTimePerSquare;
            transform
                .DOMove(currentDestination, secondsPerSquare)
                .SetEase(Ease.Linear)
                .OnComplete(StartNextSegment);
        } else {
            _unit.Running = false;
            CommitMoveToModel();
            _currentCallback();
        }
    }

    private void CommitMoveToModel() {
        var unitModel = GetComponent<Grid.Unit>().model;
        var battleModel = _battleState.Model;
        var grid = CombatObjects.GetMap();

        var gridPointPath = (from point in _currentPath
                             select grid.GridPositionForWorldPosition(point))
                             .ToList();

        var destination = gridPointPath.Last();

        battleModel.MoveUnit(unitModel, gridPointPath, destination);
    }
}