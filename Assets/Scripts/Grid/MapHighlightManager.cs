using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapHighlightManager : Singleton<MapHighlightManager> {

    public GameObject MapHighlightPrefab;
    public Material AttackSelectionMaterial;
    public Material MovementSelectionMaterial;
    public Material HoverSelectionMaterial;
    public int BaseSortOrder = 4;
    public bool HoverSelectorEnabled { get; set; }

    private GameObject HoverHighlight;
    private readonly List<GameObject> HighlightedTiles = new List<GameObject>();
    private Grid.UnitManager UnitManager;

    private Dictionary<HighlightLevel, List<MapSelection>> SelectionsByLevel = new Dictionary<HighlightLevel, List<MapSelection>>();

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

    void Start() {
        HoverHighlight = CreateHighlight(Vector2.zero, HighlightLevel.PLAYER_HOVER);
        HoverHighlight.SetActive(false);

        UnitManager = CombatObjects.GetUnitManager();
    }

    void Update() {
        HighlightHoveredTile();
    }

    public void HighlightTiles(ICollection<Vector2> tiles, HighlightLevel level) {
        ClearHighlight();
        var createdTiles = from tile in tiles
                           select CreateHighlight(tile, level);
        HighlightedTiles.AddRange(createdTiles);
    }

    public void ClearHighlight() {
        foreach (var obj in HighlightedTiles) {
            Destroy(obj);
        }

        HighlightedTiles.Clear();
    }

    private GameObject CreateHighlight(Vector2 pos, HighlightLevel level) {

        var highlight = Instantiate(MapHighlightPrefab);
        highlight.transform.position = MapGrid.Instance.GetWorldPosForGridPos(pos);

        var highlightRenderer = highlight.GetComponent<Renderer>();
        highlightRenderer.sortingLayerName = "Default";
        highlightRenderer.sortingOrder = BaseSortOrder + (int)level;

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
        }

        return highlight;
    }


    private void HighlightHoveredTile() {
        Vector2? gridPos = MapGrid.Instance.GetMouseGridPosition();
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
