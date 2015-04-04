using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SelectingMoveLocation : CancelableCombatState {
	private MapGrid Grid;
	private BattleState State;
	private Animator Animator;
    private HashSet<Vector2> WalkableLocations;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Grid = CombatObjects.GetMap();
        State = CombatObjects.GetBattleState();
		Animator = animator;

        int mov = State.GetRemainingDistance(State.SelectedUnit);

        // So we do not confuse the pathfinder, disable the unit who is moving so they
        // are no longer on the grid.
        GameObject movingUnit = State.SelectedUnit.gameObject;
        movingUnit.SetActive(false);
        Grid.RescanGraph();

        WalkableLocations = Grid.GetWalkableTilesInRange(State.SelectedGridPosition, mov);

        movingUnit.SetActive(true);
        Grid.RescanGraph();

        Grid.SelectTiles(WalkableLocations, MapGrid.SelectionType.MOVEMENT);

		Grid.OnGridClicked += new MapGrid.GridClickHandler(HandleGridClick);
	}

	private void HandleGridClick(Vector2 location) {
        if (WalkableLocations.Contains(location)) {
            State.MovementDestination = location;
            Animator.SetTrigger("move_location_selected");
        }
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Grid.OnGridClicked -= new MapGrid.GridClickHandler(HandleGridClick);
        Grid.ClearSelection();
	}
}
