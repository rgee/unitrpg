using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MapGrid : Singleton<MapGrid> {
	
    public float tileSizeInPixels = 32f;
    public int width;
    public int height;
    public GameObject MapHighlightPrefab;
    public Material AttackSelectionMaterial;
    public Material MovementSelectionMaterial;
    public Material HoverSelectionMaterial;

    public bool HoverSelectorEnabled { get; set;  }


	public delegate void GridClickHandler(Vector2 location);
	public event GridClickHandler OnGridClicked;

    private Dictionary<Vector2, MapTile> tilesByPosition = new Dictionary<Vector2, MapTile>();
	private AstarPath Pathfinder;
    private Seeker Seeker;
    private GameObject HoverHighlight;
    private Grid.UnitManager UnitManager;
    private List<GameObject> SelectionTiles = new List<GameObject>();

    public enum SelectionType {
        MOVEMENT,
        ATTACK,
        HOVER
    }

    void Start() {
        CombatEventBus.Moves.AddListener(HandleMovement);
        HoverHighlight = CreateHighlight(new Vector2(0, 0), SelectionType.HOVER);
        HoverHighlight.SetActive(false);

        UnitManager = CombatObjects.GetUnitManager();
    }

    void OnDestroy() {
        CombatEventBus.Moves.RemoveListener(HandleMovement);
    }

    private void HandleMovement(Grid.Unit unit, Vector2 destination) {
        RescanGraph();
    }

    public void RescanGraph() {
        Pathfinder.Scan();
    }

    public void Awake() {
		Pathfinder = GetComponent<AstarPath>();
        Seeker = GetComponent<Seeker>();
    }

    private HashSet<Vector2> generateSurroundingPoints(Vector2 origin, int range) {
        float leftEdge = origin.x + range;
        float rightEdge = origin.x - range;

        float topEdge = origin.y + range;
        float bottomEdge = origin.y - range;

        HashSet<Vector2> results = new HashSet<Vector2>();
        for (int x = (int)(origin.x - range); x <= origin.x + range; x++) {
            for (int y = (int)(origin.y - range); y <= origin.y + range; y++) {
                Vector2 neighbor = new Vector2(x, y);
                if (neighbor != origin) {
                    results.Add(new Vector2(x, y));
                }
            }
        }

        return results;
    }


    private float mapRange(float fromStart, float fromEnd, float toStart, float toEnd, float value) {
        float inputRange = fromEnd - fromStart;
        float outputRange = toEnd - toStart;
        return (value - fromStart) * outputRange / inputRange + toStart;
        
    }

	public void Update() {
		if (Input.GetMouseButtonDown(0)) {
			Vector2? maybePos = GetMouseGridPosition();
			if (maybePos.HasValue && OnGridClicked != null) {
				OnGridClicked(maybePos.Value);
			}
		}

        HighlightHoveredTile();
	}

    public Vector2? GetMouseGridPosition() {
      
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GridPositionForWorldPosition(mousePos);
    }

    public Vector2 GridPositionForWorldPosition(Vector3 worldPos) {

        int tileSize = (int)tileSizeInPixels;
        float widthExtent = (width / 2f) * tileSize;
        float heightExtent = (height / 2f) * tileSize;
        Vector2 result =  new Vector3(
            (float)Math.Floor(mapRange(-widthExtent, widthExtent, 0, width, worldPos.x)),
            (float)Math.Floor(mapRange(-heightExtent, heightExtent, 0, height, worldPos.y))
        );

        return result;
    }

    public void SelectTiles(ICollection<Vector2> tiles, SelectionType type) {
        ClearSelection();
        SelectionTiles = tiles.Select((tile) => CreateHighlight(tile, type)).ToList();
    }

    private GameObject CreateHighlight(Vector2 pos, SelectionType type) {

        GameObject highlight = Instantiate(MapHighlightPrefab) as GameObject;
        highlight.transform.position = GetWorldPosForGridPos(pos);

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

    public void HighlightHoveredTile() {
        Vector2? gridPos = GetMouseGridPosition();
        if (HoverSelectorEnabled && gridPos.HasValue) {
            Vector3 worldPosition = GetWorldPosForGridPos(gridPos.Value);
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

    public void ClearSelection() {
        foreach (GameObject obj in SelectionTiles) {
            Destroy(obj);
        }

        SelectionTiles.Clear();
    }

    public HashSet<Vector2> GetWalkableTilesInRange(Vector2 origin, int range) {
        HashSet<Vector2> openTiles = new RangeFinder(this).GetOpenTilesInRange(origin, range);
        HashSet<Vector3> openWorldPositions = openTiles.Select(tile => GetWorldPosForGridPos(tile)).ToHashSet();

        HashSet<Vector2> validGridPositions = new HashSet<Vector2>();
        Vector3 originWorldPosition = GetWorldPosForGridPos(origin);

        Pathfinding.GraphNode startNode = AstarPath.active.GetNearest(originWorldPosition).node;
        List<Pathfinding.GraphNode> bfsResult = Pathfinding.PathUtilities.BFS(startNode, range);

        return bfsResult.Select((node) => {
            return GridPositionForWorldPosition((Vector3)node.position);
        }).ToHashSet();
    }

    public Vector3 GetWorldPosForGridPos(Vector2 gridPos) {
        if (!IsInGrid(gridPos)) {
            return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        int tileSize = (int)tileSizeInPixels;
        float widthExtent = (width/2f)*tileSize;
        float heightExtent = (height/2f)*tileSize;

        float halfTileSize = tileSizeInPixels / 2; 

        // Map the input values for the x and y axis in grid space to world space.
        // Be sure to output the center of the tile in world space by adding
        // 1/2 the tile height and width!
        Vector3 result = new Vector3(
            mapRange(0, width, -widthExtent, widthExtent, gridPos.x) + halfTileSize,
            mapRange(0, height, -heightExtent, heightExtent, gridPos.y) + halfTileSize, 
            0
        );

        return result;
    }

    public bool IsInGrid(Vector2 gridPos) {
        return gridPos.x >= 0 && gridPos.x < width &&
               gridPos.y < height && gridPos.y >= 0;
    }
}