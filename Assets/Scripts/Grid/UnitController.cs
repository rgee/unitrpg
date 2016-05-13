using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Models.Combat;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Grid.Unit))]
public class UnitController : MonoBehaviour {
    private BattleState _battleState;
    private Grid.Unit _unit;

    private MapGrid _grid;

    private class DummyMovementEventHandler : IMovementEventHandler {
        public IEnumerator HandleMovement(Grid.Unit unit, Vector2 destination) {
            yield return null;
        }
    }

    public void Awake() {
        _unit = GetComponent<Grid.Unit>();
        _battleState = CombatObjects.GetBattleState();
        _grid = CombatObjects.GetMap();
    }

    public IEnumerator FollowPath(List<Vector3> path) {
        yield return StartCoroutine(FollowPath(path, new DummyMovementEventHandler()));
    } 

    public IEnumerator FollowPath(List<Vector3> path, IMovementEventHandler movementEventHandler) {
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

            // This isn't always used and is on the chopping block for StrangeIoC stuff.
            if (_grid != null) {
                var gridPosition = _grid.GridPositionForWorldPosition(currentDestination);
                yield return StartCoroutine(movementEventHandler.HandleMovement(_unit, gridPosition));
            }

            pathIndex++;
            previousPoint = currentDestination;
        }
        _unit.Running = false;
        CommitMoveToModel(path);
    }

    private void CommitMoveToModel(IEnumerable<Vector3> path) {
        if (_battleState == null) {
            return;
        }

        var unitModel = _unit.model;
        var battleModel = _battleState.Model;
        if (battleModel == null) {
            Debug.LogError("Could not find battle state to commit movement.");
            return;
        }

        var gridPointPath = path.Select(point => _grid.GridPositionForWorldPosition(point))
            .ToList();

        var destination = gridPointPath.Last();

        battleModel.MoveUnit(unitModel, gridPointPath, destination);
    }
}