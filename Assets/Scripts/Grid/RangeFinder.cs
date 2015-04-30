using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RangeFinder {
    private readonly MapGrid grid;

    public RangeFinder(MapGrid grid) {

        this.grid = grid;
    }

    public HashSet<Vector2> GetOpenTilesInRange(Vector2 origin, int range) {

        return generateSurroundingPoints(origin, range)
            .Where((point) => InRange(point, origin, range) && IsOpen(point))
            .ToHashSet();
    }

    public HashSet<Vector2> GetTilesInRange(Vector2 origin, int range) {

        return generateSurroundingPoints(origin, range)
            .Where((point) => InRange(point, origin, range))
            .ToHashSet();
    }

    private bool IsOpen(Vector2 point) {
        if (!grid.IsInGrid(point)) {
            return false;
        }

        Vector2 worldPosition = grid.GetWorldPosForGridPos(point);
        Pathfinding.NNInfo nearest = AstarPath.active.GetNearest(worldPosition, PathfindingUtils.GetMainGraphConstraint());
        return nearest.node.Walkable;
    }

    private bool InRange(Vector2 point, Vector2 origin, int range) {

        float dist = MathUtils.ManhattanDistance(point, origin);

        return dist <= range;
    }


    private HashSet<Vector2> generateSurroundingPoints(Vector2 origin, int range) {

        float leftEdge = origin.x + range;
        float rightEdge = origin.x - range;

        float topEdge = origin.y + range;
        float bottomEdge = origin.y - range;

        HashSet<Vector2> results = new HashSet<Vector2>();
        for (int x = (int)(origin.x - range); x <= origin.x + range; x++)
        {
            for (int y = (int)(origin.y - range); y <= origin.y + range; y++)
            {
                Vector2 point = new Vector2(x, y);
                if (point != origin) {
                    results.Add(point);
                }
            }
        }

        return results;
    }
}
