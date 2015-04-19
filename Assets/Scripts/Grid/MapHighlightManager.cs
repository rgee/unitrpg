using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapHighlightManager : Singleton<MapHighlightManager> {

    public GameObject MapHighlightPrefab;
    public Material AttackSelectionMaterial;
    public Material MovementSelectionMaterial;
    public Material HoverSelectionMaterial;
    public int BaseSortOrder = 4;
    public bool HoverSelectorEnabled { get; set;  }

    private GameObject HoverHighlight;
    private List<GameObject> HighlightedTiles = new List<GameObject>();
    private Grid.UnitManager UnitManager;

    private Dictionary<HighlightLevel, List<Selection>> SelectionsByLevel = new Dictionary<HighlightLevel, List<Selection>>();

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

    public enum HiglightType {
        MOVEMENT,
        ATTACK,
        HOVER
    }

    void Start() {
        HoverHighlight = CreateHighlight(Vector2.zero, HiglightType.HOVER);
        HoverHighlight.SetActive(false);

        UnitManager = CombatObjects.GetUnitManager();
    }

    void Update() {
        HighlightHoveredTile();
    }

    public void HighlightTiles(ICollection<Vector2> tiles, HighlightLevel level) {
        ClearHighlight();
    }

    public void HighlightTiles(ICollection<Vector2> tiles, HiglightType type) {
        ClearHighlight();

        HashSet<GameObject> createdTiles = tiles.Select((tile) => CreateHighlight(tile, type)).ToHashSet();
        Selection sel = new Selection { Name = "test", Tiles = createdTiles };
    }

    public void ClearHighlight() {
        foreach (GameObject obj in HighlightedTiles) {
            Destroy(obj);
        }

        HighlightedTiles.Clear();
    }

    private GameObject CreateHighlight(Vector2 pos, HighlightLevel level) {

        GameObject highlight = Instantiate(MapHighlightPrefab) as GameObject;
        highlight.transform.position = MapGrid.Instance.GetWorldPosForGridPos(pos);

        Renderer renderer = highlight.GetComponent<Renderer>();
        renderer.sortingLayerName = "Default";
        renderer.sortingOrder = BaseSortOrder + (int)level;

        switch (level) {
            case HighlightLevel.PLAYER_MOVE:
                renderer.material = MovementSelectionMaterial;
                break;
            case HighlightLevel.PLAYER_ATTACK:
                renderer.material = AttackSelectionMaterial;
                break;
            case HighlightLevel.PLAYER_HOVER:
                renderer.material = HoverSelectionMaterial;
                break;
        }

        return highlight;
    }

    private GameObject CreateHighlight(Vector2 pos, HiglightType type) {

        GameObject highlight = Instantiate(MapHighlightPrefab) as GameObject;
        highlight.transform.position = MapGrid.Instance.GetWorldPosForGridPos(pos);

        Renderer renderer = highlight.GetComponent<Renderer>();
        renderer.sortingLayerName = "Default";
        renderer.sortingOrder = BaseSortOrder;
        if (type == HiglightType.ATTACK) {
            renderer.material = AttackSelectionMaterial;
        } else if (type == HiglightType.MOVEMENT) {
            renderer.material = MovementSelectionMaterial;
        } else if (type == HiglightType.HOVER) {
            renderer.material = HoverSelectionMaterial;
        }

        return highlight;
    }


    private void HighlightHoveredTile() {
        Vector2? gridPos = MapGrid.Instance.GetMouseGridPosition();
        if (HoverSelectorEnabled && gridPos.HasValue) {
            Vector3 worldPosition = MapGrid.Instance.GetWorldPosForGridPos(gridPos.Value);
            HoverHighlight.transform.position = worldPosition;

            // Okay, so.
            // If there's a unit on the square, we know it's walkable, so highlight it. Done and done.
            bool shouldHighlight = false;
            Grid.Unit occupyingUnit = UnitManager.GetUnitByPosition(gridPos.Value);
            if (occupyingUnit != null) {
                shouldHighlight = true;

            // If not, we have to ensure that it's walkable by checking the graph.
            } else {
                Pathfinding.GraphNode nearestNode = AstarPath.active.GetNearest(worldPosition).node;
                shouldHighlight = nearestNode != null && nearestNode.Walkable;
            }

            HoverHighlight.SetActive(shouldHighlight);
        } else {
            HoverHighlight.SetActive(false);
        }
    }
}
