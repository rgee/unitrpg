using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;

public class MapBehavior : GridBehaviour<RectPoint> {

	public int combatForecastGutter = 20;

	private GameObject mapSelector;
	private UnitManager units;
	private RectPoint? selectedSourcePoint;
	private bool selectorEnabled;
	private ActionMenuSpawn menuSpawner;

	public override void InitGrid ()
	{
		base.InitGrid();
		units = GameObject.Find("Units").GetComponent<UnitManager>();
		mapSelector = GameObject.Find("map_selector");
		menuSpawner = GameObject.FindGameObjectWithTag("ActionMenuSpawn").GetComponent<ActionMenuSpawn>();
	}

	public void disableSelector() {
		mapSelector.SetActive(false);
	}

	public void enableSelector() {
		mapSelector.SetActive(true);
	}

	public void Update() {
		if (Input.GetKeyDown (KeyCode.H)) {
		}
	}

	private void OnClick(RectPoint point) {
		if (selectedSourcePoint == null) {
			MapCellScript clickedCell = (MapCellScript)Grid.GetCell(point);
			Unit unit = clickedCell.GetUnit();
			if (unit == null || unit.GetArmy() != Character.Army.PLAYER) {
				return;
			}

			selectedSourcePoint = point;

			HashSet<RectPoint> movableNeighbors = Algorithms.GetConnectedSet(Grid, point, 
			                                                                 (p1, p2) => {
				MapCellScript destinationCell = (MapCellScript)Grid.GetCell(p2);

				bool passable = destinationCell.passable && !destinationCell.IsOccupied();

				// Todo: Get this from the unit at the cell
				return MathUtils.ManhattanDistance(point.X, point.Y, p2.X, p2.Y) <= 4 && passable;
			});

			foreach (RectPoint neighbor in movableNeighbors) {
				MapCellScript cell = (MapCellScript)Grid.GetCell(neighbor);
				cell.Color = Color.blue;
			}
		} else {
			units.MoveUnit(selectedSourcePoint.GetValueOrDefault(), point);
			foreach (RectPoint rect in Grid) {
				Grid.GetCell(rect).Color = Color.white;
			}

			selectedSourcePoint = null;
        }
	}
}
