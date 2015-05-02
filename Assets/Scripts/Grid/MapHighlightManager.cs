using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;

public class MapHighlightManager : Singleton<MapHighlightManager> {
    /**
     * Levels defined in first to last in render order.
     */

    public enum HighlightLevel {
        GLOBAL_ENEMY_MOVE = 0,
        SPECIFIC_ENEMY_MOVE,
        PLAYER_MOVE,
        PLAYER_ATTACK,
        PLAYER_HOVER
    }

    private readonly Dictionary<string, MapSelection> SelectionsByName = new Dictionary<string, MapSelection>();
    public Material AttackSelectionMaterial;
    public int BaseSortOrder = 4;
    public Material GlobalEnemyMaterial;
    private GameObject HoverHighlight;
    public Material HoverSelectionMaterial;
    public GameObject MapHighlightPrefab;
    public Material MovementSelectionMaterial;
    public Material SpecificEnemyMaterial;
    private UnitManager UnitManager;
    public bool HoverSelectorEnabled { get; set; }

    private void Start() {
        HoverHighlight = CreateHighlight(Vector2.zero, HighlightLevel.PLAYER_HOVER);
        HoverHighlight.SetActive(false);

        UnitManager = CombatObjects.GetUnitManager();
    }

    private void Update() {
        HighlightHoveredTile();
    }

    public void HighlightTiles(ICollection<Vector2> tiles, HighlightLevel level, string name) {
        ClearHighlight(name);
        var createdTiles = tiles.Select(tile => CreateHighlight(tile, level)).ToHashSet();
        var selection = new MapSelection {Name = name, Tiles = createdTiles};
        SelectionsByName[name] = selection;
    }

    public void ClearHighlight(string name) {
        if (!SelectionsByName.ContainsKey(name)) {
            return;
        }

        foreach (var obj in SelectionsByName[name].Tiles) {
            Destroy(obj);
        }
    }

    private GameObject CreateHighlight(Vector2 pos, HighlightLevel level) {
        var highlight = Instantiate(MapHighlightPrefab);
        highlight.transform.position = MapGrid.Instance.GetWorldPosForGridPos(pos);

        var highlightRenderer = highlight.GetComponent<Renderer>();
        highlightRenderer.sortingLayerName = "Default";
        highlightRenderer.sortingOrder = BaseSortOrder + (int) level;

        switch (level) {
            case HighlightLevel.PLAYER_MOVE:
                highlightRenderer.material = MovementSelectionMaterial;
                break;
            case HighlightLevel.PLAYER_ATTACK:
                highlightRenderer.material = AttackSelectionMaterial;
                break;
            case HighlightLevel.PLAYER_HOVER:
                highlightRenderer.material = HoverSelectionMaterial;
                break;
            case HighlightLevel.SPECIFIC_ENEMY_MOVE:
                highlightRenderer.material = SpecificEnemyMaterial;
                break;
            case HighlightLevel.GLOBAL_ENEMY_MOVE:
                highlightRenderer.material = GlobalEnemyMaterial;
                break;
        }

        return highlight;
    }

    private void HighlightHoveredTile() {
        var gridPos = MapGrid.Instance.GetMouseGridPosition();
        if (HoverSelectorEnabled && gridPos.HasValue) {
            var worldPosition = MapGrid.Instance.GetWorldPosForGridPos(gridPos.Value);
            HoverHighlight.transform.position = worldPosition;

            // Okay, so.
            // If there's a unit on the square, we know it's walkable, so highlight it. Done and done.
            var shouldHighlight = false;
            var occupyingUnit = UnitManager.GetUnitByPosition(gridPos.Value);
            if (occupyingUnit != null) {
                shouldHighlight = true;

                // If not, we have to ensure that it's walkable by checking the graph.
            } else {
                var nearestNode = AstarPath.active.GetNearest(worldPosition).node;
                shouldHighlight = nearestNode != null && nearestNode.Walkable;
            }

            HoverHighlight.SetActive(shouldHighlight);
        } else {
            HoverHighlight.SetActive(false);
        }
    }
}