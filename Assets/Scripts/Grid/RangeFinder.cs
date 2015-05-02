using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder {
    private readonly MapGrid grid;

    public RangeFinder(MapGrid grid) {
        this.grid = grid;
    }

    public HashSet<Vector2> GetOpenTilesInRange(Vector2 origin, int range) {
        return generateSurroundingPoints(origin, range)
            .Where(point => InRange(point, origin, range) && IsOpen(point))
            .ToHashSet();
    }

    public HashSet<Vector2> GetTilesInRange(Vector2 origin, int range) {
        return generateSurroundingPoints(origin, range)
            .Where(point => InRange(point, origin, range))
            .ToHashSet();
    }

    private bool IsOpen(Vector2 point) {
        if (!grid.IsInGrid(point)) {
            return false;
        }

        Vector2 worldPosition = grid.GetWorldPosForGridPos(point);
        var nearest = AstarPath.active.GetNearest(worldPosition, PathfindingUtils.GetMainGraphConstraint());
        return nearest.node.Walkable;
    }

    private bool InRange(Vector2 point, Vector2 origin, int range) {
        float dist = MathUtils.ManhattanDistance(point, origin);

        return dist <= range;
    }

    private HashSet<Vector2> generateSurroundingPoints(Vector2 origin, int range) {
        var leftEdge = origin.x + range;
        var rightEdge = origin.x - range;

        var topEdge = origin.y + range;
        var bottomEdge = origin.y - range;

        var results = new HashSet<Vector2>();
        for (var x = (int) (origin.x - range); x <= origin.x + range; x++) {
            for (var y = (int) (origin.y - range); y <= origin.y + range; y++) {
                var point = new Vector2(x, y);
                if (point != origin) {
                    results.Add(point);
                }
            }
        }

        return results;
    }
}