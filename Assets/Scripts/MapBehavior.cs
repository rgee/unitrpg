using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;

public class MapBehavior : GridBehaviour<RectPoint> {

	public int combatForecastGutter = 20;

	private GameObject mapSelector;
	private UnitManager units;

	private Unit selectedUnit;
	private RectPoint? selectedSourcePoint;
	private bool selectorEnabled;

	public override void InitGrid ()
	{
		base.InitGrid();
		units = GameObject.Find("Units").GetComponent<UnitManager>();
		mapSelector = GameObject.Find("map_selector");
	}

	public void disableSelector() {
		mapSelector.SetActive(false);
	}

	public void enableSelector() {
		mapSelector.SetActive(true);
	}

	public void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			ClearMovementRange();
		}
	}

	private void ClearMovementRange() {
		selectedSourcePoint = null;
		selectedUnit = null;
		foreach (RectPoint rect in Grid) {
			Grid.GetCell(rect).Color = Color.white;
		}
	}
	
	public Unit GetSelectedUnit() {
		return selectedUnit;
	}

	private void OnClick(RectPoint point) {
		MapCellScript clickedCell = (MapCellScript)Grid.GetCell(point);
		if (selectedSourcePoint == null) {
			Unit unit = clickedCell.GetUnit();
			int movement = unit.GetMovement();
			if (unit == null || unit.IsEnemy()) {
				return;
			}
			selectedUnit = unit;
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
			units.MoveUnit(selectedUnit, selectedSourcePoint.GetValueOrDefault(), point);
			ClearMovementRange();
        }
	}
}
