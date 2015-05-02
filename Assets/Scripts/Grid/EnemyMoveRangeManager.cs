using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMoveRangeManager : Singleton<EnemyMoveRangeManager> {
    private static readonly string GLOBAL_MOVE_RANGE_NAME = "_global_move_range_";

    private readonly Dictionary<Grid.Unit, MoveRangePreviewState> StatesByUnit =
        new Dictionary<Grid.Unit, MoveRangePreviewState>();

    private bool GlobalMoveRangeActive;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            if (GlobalMoveRangeActive) {
                HideAllMoveRanges();
            } else {
                ShowAllEnemyMoveRanges();
            }
        }
    }

    public void ShowUnitMoveRange(Grid.Unit unit) {
        MoveRangePreviewState state;
        if (StatesByUnit.ContainsKey(unit)) {
            state = StatesByUnit[unit];
            var nextType = state.GetNextType();
            state.Type = nextType;

            MapHighlightManager.Instance.ClearHighlight(state.SelectionName);

            var tiles = GetWalkableTilesForPreviewType(unit, nextType);
            MapHighlightManager.Instance.HighlightTiles(tiles, MapHighlightManager.HighlightLevel.SPECIFIC_ENEMY_MOVE,
                state.SelectionName);
            StatesByUnit[unit] = state;
        } else {
            state = new MoveRangePreviewState {
                SelectionName = Guid.NewGuid().ToString(),
                Type = MoveRangePreviewState.PreviewType.OFF
            };
            StatesByUnit[unit] = state;
            ShowUnitMoveRange(unit);
        }
    }

    private HashSet<Vector2> GetWalkableTilesForPreviewType(Grid.Unit unit, MoveRangePreviewState.PreviewType type) {
        switch (type) {
            case MoveRangePreviewState.PreviewType.OFF:
                return new HashSet<Vector2>();
            case MoveRangePreviewState.PreviewType.RANGE_WITHOUT_OBSTACLES:
                return GetWalkableTilesFromUnit(unit, true);
            case MoveRangePreviewState.PreviewType.RANGE_WITH_OBSTACLES:
                return GetWalkableTilesFromUnit(unit);
        }
        throw new ArgumentException("Invalid preview type");
    }

    private HashSet<Vector2> GetWalkableTilesFromUnit(Grid.Unit unit, bool ignoreUnits = false) {
        var grid = CombatObjects.GetMap();

        unit.gameObject.SetActive(false);
        grid.RescanGraph();
        var tiles = grid.GetWalkableTilesInRange(unit.gridPosition, unit.model.Character.Movement, ignoreUnits);
        tiles.Remove(unit.gridPosition);
        unit.gameObject.SetActive(true);
        grid.RescanGraph();

        return tiles;
    }

    public void HideUnitMoveRange(Grid.Unit unit) {
        if (StatesByUnit.ContainsKey(unit)) {
            var state = StatesByUnit[unit];
            state.Type = MoveRangePreviewState.PreviewType.OFF;
            var tiles = GetWalkableTilesForPreviewType(unit, MoveRangePreviewState.PreviewType.OFF);
            MapHighlightManager.Instance.HighlightTiles(tiles, MapHighlightManager.HighlightLevel.SPECIFIC_ENEMY_MOVE,
                state.SelectionName);
        }
    }

    public void HideAllMoveRanges() {
        MapHighlightManager.Instance.ClearHighlight(GLOBAL_MOVE_RANGE_NAME);
        GlobalMoveRangeActive = false;
    }

    public void ShowAllEnemyMoveRanges() {
        var unitManager = CombatObjects.GetUnitManager();

        var walkableTiles = unitManager
            .GetEnemies()
            .SelectMany(unit => GetWalkableTilesFromUnit(unit))
            .ToHashSet();

        var mapHighlightManager = MapHighlightManager.Instance;
        mapHighlightManager.HighlightTiles(walkableTiles, MapHighlightManager.HighlightLevel.GLOBAL_ENEMY_MOVE,
            GLOBAL_MOVE_RANGE_NAME);
        GlobalMoveRangeActive = true;
    }
}