using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Pathfinding;
using Tiled2Unity;
using UnityEngine;

public class MapGrid : Singleton<MapGrid> {
    public delegate void GridClickHandler(Vector2 location);

    public int height;
    private AstarPath Pathfinder;
    private Seeker Seeker;
    private Dictionary<Vector2, MapTile> tilesByPosition = new Dictionary<Vector2, MapTile>();
    public float tileSizeInPixels = 32f;
    private UnitManager UnitManager;
    public int width;
    public event GridClickHandler OnGridClicked;

    private void Start() {
        CombatEventBus.Moves.AddListener(HandleMovement);
        UnitManager = CombatObjects.GetUnitManager();
    }

    private void OnDestroy() {
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

        var tiledMap = GetComponent<TiledMap>();
        if (tiledMap != null) {
            tiledMap.NumTilesHigh = height;
            tiledMap.NumTilesWide = width;
            tiledMap.TileHeight = (int) tileSizeInPixels;
            tiledMap.TileWidth = (int) tileSizeInPixels;
        }
    }

    private HashSet<Vector2> generateSurroundingPoints(Vector2 origin, int range) {
        var leftEdge = origin.x + range;
        var rightEdge = origin.x - range;

        var topEdge = origin.y + range;
        var bottomEdge = origin.y - range;

        var results = new HashSet<Vector2>();
        for (var x = (int) (origin.x - range); x <= origin.x + range; x++) {
            for (var y = (int) (origin.y - range); y <= origin.y + range; y++) {
                var neighbor = new Vector2(x, y);
                if (neighbor != origin) {
                    results.Add(new Vector2(x, y));
                }
            }
        }

        return results;
    }

    private float mapRange(float fromStart, float fromEnd, float toStart, float toEnd, float value) {
        var inputRange = fromEnd - fromStart;
        var outputRange = toEnd - toStart;
        return (value - fromStart)*outputRange/inputRange + toStart;
    }

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            var maybePos = GetMouseGridPosition();
            if (maybePos.HasValue && OnGridClicked != null) {
                OnGridClicked(maybePos.Value);
            }
        }
    }

    public Vector2? GetMouseGridPosition() {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GridPositionForWorldPosition(mousePos);
    }

    public Vector2 GridPositionForWorldPosition(Vector3 worldPos) {
        var tileSize = (int) tileSizeInPixels;
        var widthExtent = (width/2f)*tileSize;
        var heightExtent = (height/2f)*tileSize;
        Vector2 result = new Vector3(
            (float) Math.Floor(mapRange(-widthExtent, widthExtent, 0, width, worldPos.x)),
            (float) Math.Floor(mapRange(-heightExtent, heightExtent, 0, height, worldPos.y))
            );

        return result;
    }

    public HashSet<Vector2> GetWalkableTilesInRange(Vector2 origin, int range, bool ignoreOtherUnits = false) {
        var searchConstraint = ignoreOtherUnits
            ? PathfindingUtils.GetUnitlessGraphConstraint()
            : PathfindingUtils.GetMainGraphConstraint();

        var originWorldPosition = GetWorldPosForGridPos(origin);

        var startNode = AstarPath.active.GetNearest(originWorldPosition, searchConstraint).node;
        var bfsResult = PathUtilities.BFS(startNode, range);

        return bfsResult.Select(node => { return GridPositionForWorldPosition((Vector3) node.position); }).ToHashSet();
    }

    public Vector3 GetWorldPosForGridPos(Vector2 gridPos) {
        if (!IsInGrid(gridPos)) {
            return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        }

        var tileSize = (int) tileSizeInPixels;
        var widthExtent = (width/2f)*tileSize;
        var heightExtent = (height/2f)*tileSize;

        var halfTileSize = tileSizeInPixels/2;

        // Map the input values for the x and y axis in grid space to world space.
        // Be sure to output the center of the tile in world space by adding
        // 1/2 the tile height and width!
        var result = new Vector3(
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