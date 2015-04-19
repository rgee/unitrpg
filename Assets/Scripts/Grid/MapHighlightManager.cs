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
    public bool HoverSelectorEnabled { get; set;  }

    private GameObject HoverHighlight;
    private List<GameObject> SelectionTiles = new List<GameObject>();
    private Grid.UnitManager UnitManager;

    public enum SelectionType {
        MOVEMENT,
        ATTACK,
        HOVER
    }

    void Start() {
        HoverHighlight = CreateHighlight(new Vector2(0, 0), SelectionType.HOVER);
        HoverHighlight.SetActive(false);

        UnitManager = CombatObjects.GetUnitManager();
    }

    void Update() {
        HighlightHoveredTile();
    }

    public void SelectTiles(ICollection<Vector2> tiles, SelectionType type) {
        ClearSelection();
        SelectionTiles = tiles.Select((tile) => CreateHighlight(tile, type)).ToList();
    }

    public void ClearSelection() {
        foreach (GameObject obj in SelectionTiles) {
            Destroy(obj);
        }

        SelectionTiles.Clear();
    }

    private GameObject CreateHighlight(Vector2 pos, SelectionType type) {

        GameObject highlight = Instantiate(MapHighlightPrefab) as GameObject;
        highlight.transform.position = MapGrid.Instance.GetWorldPosForGridPos(pos);

        Renderer renderer = highlight.GetComponent<Renderer>();
        renderer.sortingLayerName = "Default";
        renderer.sortingOrder = 4;
        if (type == SelectionType.ATTACK) {
            renderer.material = AttackSelectionMaterial;
        } else if (type == SelectionType.MOVEMENT) {
            renderer.material = MovementSelectionMaterial;
        } else if (type == SelectionType.HOVER) {
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
