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
		Grid = GameObject.Find("Grid").GetComponent<MapGrid>();
		State = GameObject.Find("BattleManager").GetComponent<BattleState>();
		Animator = animator;

		int mov = State.SelectedUnit.model.Character.Movement;
		HashSet<MapTile> walkableTiles = Grid.GetWalkableTilesInRange(State.SelectedGridPosition, mov);
        Grid.SelectTiles(walkableTiles, Color.blue);

        WalkableLocations = walkableTiles.Select(tile => tile.gridPosition).ToHashSet();
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
