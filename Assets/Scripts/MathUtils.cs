using UnityEngine;
using System.Collections;

public static class MathUtils {
	public static int ManhattanDistance(int x1, int y1, int x2, int y2) {
		return Mathf.Abs(x2 - x1) + Mathf.Abs(y2 - y1);
	}

	public enum CardinalDirection {
		N, S, E, W
	}

	public static CardinalDirection DirectionTo(Vector3 start, Vector3 end) {
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
