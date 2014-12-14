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
		MapCellScript clickedCell = (MapCellScript)Grid.GetCell(point);
		Unit unit = clickedCell.GetUnit();
		int movement = unit.GetMovement();
		Debug.Log("unit movement: " + movement);
		if (selectedSourcePoint == null) {
			if (unit == null || unit.GetArmy() != Character.Army.PLAYER) {
				return;
			}

			selectedSourcePoint = point;

			HashSet<RectPoint> movableNeighbors = Algorithms.GetConnectedSet(Grid, point, 
			                                                                 (p1, p2) => {
				MapCellScript destinationCell = (MapCellScript)Grid.GetCell(p2);

				bool passable = destinationCell.passable && !destinationCell.IsOccupied();
				int manhattanDist = MathUtils.ManhattanDistance(point.X, point.Y, p2.X, p2.Y);

				return (manhattanDist <= movement) && passable;
			});

			foreach (RectPoint neighbor in movableNeighbors) {
				MapCellScript cell = (MapCellScript)Grid.GetCell(neighbor);
				cell.Color = Color.blue;
			}
		} else {
			units.MoveUnit(unit, selectedSourcePoint.GetValueOrDefault(), point);
			foreach (RectPoint rect in Grid) {
				Grid.GetCell(rect).Color = Color.white;
			}

			selectedSourcePoint = null;
        }
	}
}
