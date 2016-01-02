using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Grid.Unit))]
public class UnitController : MonoBehaviour {
    private BattleState _battleState;
    private Grid.Unit _unit;

    public void Start() {
        _unit = GetComponent<Grid.Unit>();
        _battleState = CombatObjects.GetBattleState();
    }

    public IEnumerator FollowPath(List<Vector3> path) {
        var pathIndex = 0;
        var previousPoint = MathUtils.Round(transform.position);

        _unit.Running = true;
        while (pathIndex < path.Count) {
            var currentDestination = MathUtils.Round(path[pathIndex]);
            _unit.Facing = MathUtils.DirectionTo(previousPoint, currentDestination);

            var secondsPerSquare = _unit.model.Character.MoveTimePerSquare;
            yield return transform
                .DOMove(currentDestination, secondsPerSquare)
                .SetEase(Ease.Linear)
                .WaitForCompletion();

            pathIndex++;
            previousPoint = currentDestination;
        }
        _unit.Running = false;
        CommitMoveToModel(path);
    }

    private void CommitMoveToModel(IEnumerable<Vector3> path) {
        var unitModel = _unit.model;
        var battleModel = _battleState.Model;
        var grid = CombatObjects.GetMap();

        var gridPointPath = path.Select(point => grid.GridPositionForWorldPosition(point))
            .ToList();

        var destination = gridPointPath.Last();

        battleModel.MoveUnit(unitModel, gridPointPath, destination);
    }
}