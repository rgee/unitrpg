using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectingMoveLocation : StateMachineBehaviour {
	private MapGrid Grid;
	private BattleState State;
	private Animator Animator;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Grid = GameObject.Find("Grid").GetComponent<MapGrid>();
		State = GameObject.Find("BattleManager").GetComponent<BattleState>();
		Animator = animator;

		int mov = State.SelectedUnit.model.Character.Movement;
		HashSet<MapTile> walkableTiles = Grid.GetWalkableTilesInRange(State.SelectedGridPosition, mov);
        Grid.SelectTiles(walkableTiles, Color.blue);

		Grid.OnGridClicked += new MapGrid.GridClickHandler(HandleGridClick);
	}

	private void HandleGridClick(Vector2 location) {
		State.MovementDestination = location;
		Animator.SetTrigger("move_location_selected");
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Grid.OnGridClicked -= new MapGrid.GridClickHandler(HandleGridClick);
	}
}
