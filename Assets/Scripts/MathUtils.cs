using UnityEngine;
using System.Collections;

public static class MathUtils {
	public static int ManhattanDistance(int x1, int y1, int x2, int y2) {
		return Mathf.Abs(x2 - x1) + Mathf.Abs(y2 - y1);
	}

    public static int ManhattanDistance(Vector2 start, Vector2 end) {
        return ManhattanDistance((int)start.x, (int)start.y, (int)end.x, (int)end.y);
    }

	public enum CardinalDirection {
		N, S, E, W
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
                throw new System.ArgumentException("Invalid direction.");
        }
    }

	public static CardinalDirection DirectionTo(Vector3 start, Vector3 end) {
        return DirectionTo(new Vector2(start.x, start.y), new Vector2(end.x, end.y));
    }

    public static CardinalDirection DirectionTo(Vector2 start, Vector2 end) {

		if (start == end) {
			throw new System.ArgumentException("Points are equal!");
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
