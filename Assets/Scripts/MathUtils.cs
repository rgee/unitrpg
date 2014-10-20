using UnityEngine;
using System.Collections;

public static class MathUtils {
	public static int ManhattanDistance(int x1, int y1, int x2, int y2) {
		return Mathf.Abs(x2 - x1) + Mathf.Abs(y2 - y1);
	}
}
