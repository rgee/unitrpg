using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SelectingMoveLocation : CancelableCombatState {
    public GameObject MovementPipPrefab;

	private MapGrid Grid;
	private BattleState State;
	private Animator Animator;
    private HashSet<Vector2> WalkableLocations;
    private GameObject MovementPipDialogObject;
    private MovementDialog MovementPipDialogComponent;
    private Seeker SelectedSeeker;
    private int UsedDistance;
    private Vector2 LastHoveredGridPoint;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateEnter(animator, stateInfo, layerIndex);

        Grid = CombatObjects.GetMap();
        State = CombatObjects.GetBattleState();
		Animator = animator;

        int mov = State.GetRemainingDistance(State.SelectedUnit);
        UsedDistance = State.GetUsedDistance(State.SelectedUnit);

        // So we do not confuse the pathfinder, disable the unit who is moving so they
        // are no longer on the grid.
        GameObject movingUnit = State.SelectedUnit.gameObject;
        movingUnit.SetActive(false);
        Grid.RescanGraph();

        WalkableLocations = Grid.GetWalkableTilesInRange(State.SelectedGridPosition, mov);
        WalkableLocations.Remove(State.SelectedGridPosition);

        movingUnit.SetActive(true);
        Grid.RescanGraph();

        Grid.SelectTiles(WalkableLocations, MapGrid.SelectionType.MOVEMENT);

		Grid.OnGridClicked += new MapGrid.GridClickHandler(HandleGridClick);

        MovementPipDialogObject = Instantiate(MovementPipPrefab) as GameObject;
        MovementPipDialogComponent = MovementPipDialogObject.GetComponent<MovementDialog>();
        MovementPipDialogComponent.TotalMoves = State.SelectedUnit.GetComponent<Grid.Unit>().model.Character.Movement;

        SelectedSeeker = State.SelectedUnit.GetComponent<Seeker>();
	}

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        Vector2? maybeMouseGridPos = Grid.GetMouseGridPosition();
        if (maybeMouseGridPos.HasValue) {
            Vector2 mouseGridPos = maybeMouseGridPos.Value;
            if (mouseGridPos != LastHoveredGridPoint) {
                Vector3 src = State.SelectedUnit.transform.position;
                Vector3 dest = Grid.GetWorldPosForGridPos(mouseGridPos);

                BoxCollider2D collider = State.SelectedUnit.gameObject.GetComponent<BoxCollider2D>();
                collider.enabled = false;
                Grid.RescanGraph();
                SelectedSeeker.StartPath(src, dest, (p) => {
                    collider.enabled = true;
                    Grid.RescanGraph();

                    if (!p.error && p.path.First() != p.path.Last()) {
                        LastHoveredGridPoint = mouseGridPos;
                        PathArrowManager.Instance.ShowPath(p.vectorPath.ToList());

                        // Update the dialog to reflect the new path, minus one node becuase it's the start point.
                        List<Vector3> trimmedPath = p.vectorPath.GetRange(1, p.vectorPath.Count - 1);
                        MovementPipDialogComponent.UsedMoves = trimmedPath.Count + UsedDistance;
                    } else {
                        MovementPipDialogComponent.UsedMoves = UsedDistance;
                        PathArrowManager.Instance.ClearPath();
                    }
                });
            }
        }
    }

	private void HandleGridClick(Vector2 location) {
        if (WalkableLocations.Contains(location)) {
            State.MovementDestination = location;
            Animator.SetTrigger("move_location_selected");
        }
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		base.OnStateExit(animator, stateInfo, layerIndex);

        PathArrowManager.Instance.ClearPath();
		Grid.OnGridClicked -= new MapGrid.GridClickHandler(HandleGridClick);
        Grid.ClearSelection();
        Destroy(MovementPipDialogObject);
	}
}
