using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;

public class UnitManager : MonoBehaviour {
	public int gridSize = 32;
	public float moveTimeSeconds = 0.1f;
	private MapBehavior map;
	private Dictionary<IGridPoint, Transform> childrenByWorldPosition = new Dictionary<IGridPoint, Transform>();

	private enum Direction {
		South = 0,
		West,
		North,
		East
	};

	void Start () {
		map = GameObject.Find("Map").GetComponent<MapBehavior>();

		foreach (Transform child in transform) {
			RectPoint gridPoint = map.Map[child.position];

			// Snap to grid.
			child.position = map.Map[gridPoint];
			childrenByWorldPosition.Add(gridPoint, child);
		}

	}

	public IEnumerable<GameObject> getUnits() {
		List<GameObject> units = new List<GameObject>();
		foreach (Transform child in childrenByWorldPosition.Values) {
			units.Add(child.gameObject);
		}

		return units;
	}

	public void MoveUnit (Unit unit, RectPoint src, RectPoint dest) {
		StartCoroutine(SlideUnit(unit, src, dest));
	}

	private IEnumerator SlideUnit(Unit unit, RectPoint src, RectPoint dest) {

		if (childrenByWorldPosition.ContainsKey(src)) {
      		Transform child = childrenByWorldPosition[src];
			Animator childAnimator = child.GetComponent<Animator>();

			System.Func<TileCell, bool> isAccessible = (cell) => {
				MapCellScript mapCell = (MapCellScript)cell;
				return mapCell.passable && !mapCell.IsOccupied();
			};

			Debug.Log(unit.GetMovement());

			System.Func<RectPoint, RectPoint, float> cost = (p1, p2) => {
				// Ensure that no point on the path goes further than the character can actually end up.
				if (MathUtils.ManhattanDistance(src.X, src.Y, p2.X, p2.Y) > unit.GetMovement()) {
					return float.MaxValue;
				} else {
					return p1.DistanceFrom(p2);
				}
			};

			IEnumerable<RectPoint> path = Algorithms.AStar<TileCell, RectPoint>(map.Grid, src, dest, (p1, p2) => p1.DistanceFrom(p2),
			                                                                    isAccessible, cost);
			if (path == null) {
				yield return null;
			}

			childAnimator.SetBool("Running", true);
			foreach (RectPoint point in path) {
				// The first point in the path is the source, so ignore that one.
				if (point.Equals(src)) {
					continue;
				}

				float startTime = Time.time;
				Vector3 startPoint = child.position;
				Vector3 pointDestWorldSpace = map.Map[point];
				Vector3 scale = child.localScale;

				if (child.position.x > pointDestWorldSpace.x) {
					childAnimator.SetInteger("Direction", (int)Direction.West);
					scale.x = Mathf.Abs(scale.x);
        		} else if (child.position.x < pointDestWorldSpace.x) {
					childAnimator.SetInteger("Direction", (int)Direction.East);
					scale.x = Mathf.Abs(scale.x) * -1;
				} else if (child.position.y > pointDestWorldSpace.y) {
					childAnimator.SetInteger("Direction", (int)Direction.South);
					scale.x = Mathf.Abs(scale.x);
		        } else {
					childAnimator.SetInteger("Direction", (int)Direction.North);
					scale.x = Mathf.Abs(scale.x);
        		}

				child.localScale = scale;

				while (Vector3.Distance(child.position, pointDestWorldSpace) > 0.05f) {
					float timeSinceStarted = Time.time - startTime;
					float percentageComplete = timeSinceStarted / moveTimeSeconds;
					
					child.position = Vector3.Lerp(startPoint, pointDestWorldSpace, percentageComplete);
		          	yield return null;
				}
	        }

			childAnimator.SetBool("Running", false);
			childrenByWorldPosition.Remove(src);
      		childrenByWorldPosition.Add(dest, child);

			MapCellScript srcCell = (MapCellScript)map.Grid.GetCell(src);
			MapCellScript destCell = (MapCellScript)map.Grid.GetCell(dest);
			srcCell.occupyingUnit = null;
			destCell.occupyingUnit = child.gameObject;
		}
	}

}
