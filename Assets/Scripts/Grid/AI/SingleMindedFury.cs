using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * This AI brain on every turn just tries to seek out a target.
 * Can hold off until the target is within a certain range, as well.
 */
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Grid.Unit))]
public class SingleMindedFury : MonoBehaviour, AIStrategy {
    public GameObject ExecutorPrefab;
    public GameObject Target;
    public int AggroRange = int.MaxValue;
    public Grid.UnitManager UnitManager;

    private Seeker Seeker;
    private Grid.Unit Unit;
    private MapGrid Grid;
    private int AttackRange;
    private int MoveRange;

    private GridCameraController CameraController;

    public void Awake() {
        Seeker = GetComponent<Seeker>();
        Unit = GetComponent<Grid.Unit>();
        Grid = CombatObjects.GetMap();
        AttackRange = 1;
        MoveRange = Unit.model.Character.Movement;
        CameraController = CombatObjects.GetCameraController();
    }

    public IEnumerator act() {
        CameraController.Lock();
        /**
         * Strategy:
         * 
         * - If we are adjacent to the target, HIT IT
         * - Get all squares you can move to using BFS
         * - Filter down to those from which you can hit your target
         * - If there are none, move as far toward your target as you can using A*
         * - If there are some, choose one at random, move to that square, then attack.
         */

        Vector2 targetLoc = Grid.GridPositionForWorldPosition(Target.transform.position);
        Vector2 ourGridPos = Unit.gridPosition;
        if (MathUtils.ManhattanDistance(targetLoc, ourGridPos) <= AttackRange) {
            yield return StartCoroutine(AttackTarget());
        } else {
            Vector2? attackPosition = FindAttackableLocation();
            if (attackPosition.HasValue) {
                yield return StartCoroutine(MoveThenAttack(attackPosition.Value));
            } else {
                yield return StartCoroutine(ApproachTarget());
            }
        }
        CameraController.Unlock();
    }

    private Vector2? FindAttackableLocation() {

        // Find the locations that are within attack range
        Vector2 targetLoc = Grid.GridPositionForWorldPosition(Target.transform.position);
        List<Vector2> attackableLocations = (from loc in BreadthFirstSearch() 
                                             where MathUtils.ManhattanDistance(loc, targetLoc) <= AttackRange
                                             select loc).ToList();

        // If an attack and be launched from any of the locations 
        if (attackableLocations.Any()) {
            // Get the nearest one to us.
            return attackableLocations
                .OrderBy((loc) => {
                    return MathUtils.ManhattanDistance(Unit.gridPosition, loc);
                })
                .First();
        }

        return null;
    }

    private List<Vector2> BreadthFirstSearch() {

        /**
         * Sigh.
         * 
         * I'd love to find a more elegant solution to this problem, but the issue is
         * that A* Pathfinding Project's BFS implementation returns 0 results if the start
         * node is not walkable. This makes sense, but our start node is the node on which
         * we're currently standing, so it's always unwalkable. 
         * 
         * To temporarily gloss over this, we remove ourselves from the world, trigger a graph
         * update, run the search, add ourselves back and trigger another graph update.
         *
         * Hope their raycasting graph building procedure is fast...
         */
        gameObject.SetActive(false);
        Grid.RescanGraph();

        Pathfinding.GraphNode occupiedNode = AstarPath.active.GetNearest(transform.position).node;
        List<Pathfinding.GraphNode> bfsResult = Pathfinding.PathUtilities.BFS(occupiedNode, MoveRange);

        gameObject.SetActive(true);
        Grid.RescanGraph();

        List<Vector2> gridResults = (from node in bfsResult
                                     select Grid.GridPositionForWorldPosition((Vector3)node.position)).ToList();

        return gridResults;
    }

    private IEnumerator MoveThenAttack(Vector2 attackPosition) {
        Vector3 worldPos = Grid.GetWorldPosForGridPos(attackPosition);
        yield return StartCoroutine(ApproachPosition(worldPos));
        yield return StartCoroutine(AttackTarget());
    }

    private IEnumerator ApproachTarget() {
        yield return StartCoroutine(ApproachPosition(Target.transform.position));
    }
    private IEnumerator ApproachPosition(Vector3 position) {
        // Remove this unit's collider so the pathfinder wont see the currently-occupied Grid square
        // as a blockage.
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        Grid.RescanGraph();
        Pathfinding.Path path = null;
        Seeker.StartPath(transform.position, position, (p) => {
            collider.enabled = true;
            Grid.RescanGraph();
            path = p;
        }, 1 << 0);

        while (path == null) {
            yield return new WaitForEndOfFrame();
        }

		// Limit the path found to the unit's move range.
		int moveRange = Unit.model.Character.Movement;
		List<Vector3> limitedPath = path.vectorPath
            .GetRange(1, path.vectorPath.Count - 1)
            .Take(moveRange).ToList();

        foreach (Vector3 node in limitedPath) {
            Debug.Log(node);
        }

        if (!path.error) {
            yield return StartCoroutine(Unit.MoveAlongPath(limitedPath));

			Vector2 destination = Grid.GridPositionForWorldPosition(limitedPath.Last());
            CombatEventBus.Moves.Dispatch(Unit, destination);
        } else {
            yield break;
        }
    }

    private IEnumerator AttackTarget() {
        Grid.Unit targetUnit = Target.GetComponent<Grid.Unit>();
             
        Participants participants = new Participants(
            Unit.model,
            targetUnit.model
        );
        Fight fight = new Fight(participants, AttackType.BASIC, new DefaultFightResolution());
        FightResult result = fight.SimulateFight();


        MathUtils.CardinalDirection attackerDirection = MathUtils.DirectionTo(Unit.gridPosition, targetUnit.gridPosition);
        MathUtils.CardinalDirection defenderDirection = attackerDirection.GetOpposite();

        Unit.PrepareForCombat(attackerDirection);
        targetUnit.PrepareForCombat(defenderDirection);

        GameObject executorObj = Instantiate(ExecutorPrefab) as GameObject;
        FightExecutor executor = executorObj.GetComponent<FightExecutor>();
        yield return StartCoroutine(executor.RunFight(
            gameObject,
            Target,
            result
        ));
        Destroy(executorObj);
    }
}