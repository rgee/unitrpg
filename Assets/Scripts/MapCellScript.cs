using UnityEngine;
using Gamelogic.Grids;

public class MapCellScript : SpriteCell {
	public bool passable = true;
	public Transform mapSelector;
	public GameObject occupyingUnit;

	private Transform currentSelector;


	public void OnMouseOver() {
		GameObject selector = GameObject.Find("map_selector");
		if (selector != null) {
			selector.transform.parent = transform;
			selector.transform.localPosition = new Vector3(0, 0, 0);
		}
	}

	public void OnMouseExit() {
		GameObject selector = GameObject.Find("map_selector");
		if (selector != null) {
			selector.transform.parent = null;
		}
	}

	public new void OnClick() {
		// ignore
	}

	public new void OnDrawGizmos() {
	}

	public bool IsOccupied() {
		return occupyingUnit != null;
	}

	public Unit GetUnit() {
		if (occupyingUnit != null) {
			return occupyingUnit.GetComponent<Unit>();
		} else {
			return null;
		}
	}
}
