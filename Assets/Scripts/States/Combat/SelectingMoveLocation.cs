using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectingMoveLocation : CancelableCombatState {
    private static readonly string PLAYER_ATTACK_PREVIEW_RANGE = "player_attack_preview_range";
    private static readonly string PLAYER_MOVE_RANGE = "player_move_range";
    private static readonly int DEFAULT_ATTACK_RANGE = 1;
    private Animator Animator;
    private MapGrid Grid;
    private Vector2 LastHoveredGridPoint;
    private MovementDialog MovementPipDialogComponent;
    private GameObject MovementPipDialogObject;
    public GameObject MovementPipPrefab;
    private Seeker SelectedSeeker;
    private BattleState State;
    private int UsedDistance;
    private HashSet<Vector2> WalkableLocations;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Grid = CombatObjects.GetMap();
        State = CombatObjects.GetBattleState();
        Animator = animator;

        var attackRange = DEFAULT_ATTACK_RANGE;
        var mov = State.GetRemainingDistance(State.SelectedUnit);
        UsedDistance = State.GetUsedDistance(State.SelectedUnit);

        // So we do not confuse the pathfinder, disable the unit who is moving so they
        // are no longer on the grid.
        var movingUnit = State.SelectedUnit.gameObject;
        movingUnit.SetActive(false);
        Grid.RescanGraph();

        // We show walkable locations in blue, as the range over which the unit can traverse this turn
        WalkableLocations = Grid.GetWalkableTilesInRange(State.SelectedGridPosition, mov);

        // We also show the squares the unit can attack if they moved to the fringes of their move range
        // To compute this, we find all squares within (atk_range + mov_range) of the unit, ignoring
        // other units as obstacles.
        // 
        // Then, because we do not want to overlap squares here, we exclude all squares from the move range
        // Finally, we filter out any squares that are not within atk_range of any square from the move range.
        //
        // This is because the unit-ignoring grid search for squares is unhindered by other units, so it's a 
        // superset of the attackable squares a unit can reach just by walking, but we also need to include
        // squares occupied by the enemy in the results.
        var AttackableLocations = Grid.GetWalkableTilesInRange(State.SelectedGridPosition, mov + attackRange, true);
        AttackableLocations = AttackableLocations
            .Except(WalkableLocations)
            .Where(loc => {
                // Where this location is within attack range of any Walkable point.
                return
                    WalkableLocations.Any(
                        walkable => { return MathUtils.ManhattanDistance(walkable, loc) == attackRange; });
            })
            .ToHashSet();

        WalkableLocations.Remove(State.SelectedGridPosition);
        AttackableLocations.Remove(State.SelectedGridPosition);

        movingUnit.SetActive(true);
        Grid.RescanGraph();

        var highlights = MapHighlightManager.Instance;
        highlights.HighlightTiles(AttackableLocations, MapHighlightManager.HighlightLevel.PLAYER_ATTACK,
            PLAYER_ATTACK_PREVIEW_RANGE);
        highlights.HighlightTiles(WalkableLocations, MapHighlightManager.HighlightLevel.PLAYER_MOVE, PLAYER_MOVE_RANGE);

        Grid.OnGridClicked += HandleGridClick;

        MovementPipDialogObject = Instantiate(MovementPipPrefab);
        MovementPipDialogComponent = MovementPipDialogObject.GetComponent<MovementDialog>();
        MovementPipDialogComponent.TotalMoves = State.SelectedUnit.GetComponent<Grid.Unit>().model.Character.Movement;

        SelectedSeeker = State.SelectedUnit.GetComponent<Seeker>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        // If we're in the middle of cancelling this state, we will have deselected the unit,
        // so we cannot update the movement arrow anymore. 
        if (State.SelectedUnit == null) {
            return;
        }

        var maybeMouseGridPos = Grid.GetMouseGridPosition();
        if (maybeMouseGridPos.HasValue) {
            var mouseGridPos = maybeMouseGridPos.Value;
            if (mouseGridPos != LastHoveredGridPoint) {
                if (!WalkableLocations.Contains(mouseGridPos)) {
                    return;
                }

                var unit = State.SelectedUnit;
                var src = unit.transform.position;
                var dest = Grid.GetWorldPosForGridPos(mouseGridPos);

                var collider = State.SelectedUnit.gameObject.GetComponent<BoxCollider2D>();
                collider.enabled = false;
                Grid.RescanGraph();
                SelectedSeeker.StartPath(src, dest, p => {
                    collider.enabled = true;
                    Grid.RescanGraph();

                    if (!p.error && p.path.First() != p.path.Last()) {
                        LastHoveredGridPoint = mouseGridPos;
                        PathArrowManager.Instance.ShowPath(p.vectorPath.ToList());

                        // Update the dialog to reflect the new path, minus one node becuase it's the start point.
                        var trimmedPath = p.vectorPath.GetRange(1, p.vectorPath.Count - 1);
                        MovementPipDialogComponent.UsedMoves = trimmedPath.Count + UsedDistance;
                    } else {
                        MovementPipDialogComponent.UsedMoves = UsedDistance;
                        PathArrowManager.Instance.ClearPath();
                    }
                }, 1 << 0);
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
        Grid.OnGridClicked -= HandleGridClick;

        MapHighlightManager.Instance.ClearHighlight(PLAYER_MOVE_RANGE);
        MapHighlightManager.Instance.ClearHighlight(PLAYER_ATTACK_PREVIEW_RANGE);

        Destroy(MovementPipDialogObject);
    }
}