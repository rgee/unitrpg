using System;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils {
    public enum CardinalDirection {
        N,
        S,
        E,
        W
    }

    public enum Orientation {
        HORIZONTAL,
        VERTICAL
    }

    public static IEnumerable<Vector2> GetAdjacentPoints(Vector2 point) {
        return new List<Vector2> {
            new Vector2(point.x-1, point.y),
            new Vector2(point.x+1, point.y),
            new Vector2(point.x, point.y-1),
            new Vector2(point.x, point.y+1)
        };
    }

    public static int ManhattanDistance(int x1, int y1, int x2, int y2) {
        return Mathf.Abs(x2 - x1) + Mathf.Abs(y2 - y1);
    }

    public static int ManhattanDistance(Vector2 start, Vector2 end) {
        return ManhattanDistance((int) start.x, (int) start.y, (int) end.x, (int) end.y);
    }

    public static CardinalDirection GetOpposite(this CardinalDirection dir) {
        switch (dir) {
            case CardinalDirection.N:
                return CardinalDirection.S;
            case CardinalDirection.S:
                return CardinalDirection.N;
            case CardinalDirection.W:
                return CardinalDirection.E;
            case CardinalDirection.E:
                return CardinalDirection.W;
            default:
                throw new ArgumentException("Invalid direction.");
        }
    }

    public static Orientation GetOrientation(this CardinalDirection dir) {
        switch (dir) {
            case CardinalDirection.W:
            case CardinalDirection.E:
                return Orientation.HORIZONTAL;
            case CardinalDirection.S:
            case CardinalDirection.N:
                return Orientation.VERTICAL;
            default:
                throw new ArgumentException("Invalid direction.");
        }
    }

    public static CardinalDirection DirectionTo(Vector3 start, Vector3 end) {
        return DirectionTo(new Vector2(start.x, start.y), new Vector2(end.x, end.y));
    }

    public static Vector3 Round(Vector3 vec) {
        return new Vector3(
            (int)Math.Round(vec.x),
            (int)Math.Round(vec.y),
            (int)Math.Round(vec.z)
        );
    }

    public static CardinalDirection DirectionTo(Vector2 start, Vector2 end) {
        if (start == end) {
            throw new ArgumentException("Points " + start + " and " + end + " are equal!");
        }

        if (start.x < end.x) {
            return CardinalDirection.E;
        }

        if (start.x > end.x) {
            return CardinalDirection.W;
        }

        if (start.y < end.y) {
            return CardinalDirection.N;
        }

        return CardinalDirection.S;
    }
}