using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SelectingFightTarget : StateMachineBehaviour {
    private MapGrid Grid;
    private BattleState State;
    private Animator Animator;
    private Grid.UnitManager UnitManager;
    private HashSet<Vector2> AttackableLocations;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Grid = GameObject.Find("Grid").GetComponent<MapGrid>();
        State = GameObject.Find("BattleManager").GetComponent<BattleState>();
        UnitManager = GameObject.Find("Unit Manager").GetComponent<Grid.UnitManager>();
        Animator = animator;

        RangeFinder rangeFinder = new RangeFinder(Grid);
        HashSet<MapTile> tiles = rangeFinder.GetTilesInRange(State.SelectedGridPosition, 1)
            .Where(tile => UnitManager.GetUnitByPosition(tile.gridPosition) != null)
            .ToHashSet();

        AttackableLocations = tiles.Select(tile => tile.gridPosition).ToHashSet();

        Grid.SelectTiles(tiles, Color.red);

        Grid.OnGridClicked += new MapGrid.GridClickHandler(HandleGridClick);
    }

    private void HandleGridClick(Vector2 loc) {
        if (AttackableLocations.Contains(loc)) {
            Grid.Unit target = UnitManager.GetUnitByPosition(loc);
            if (target != null) {
                State.AttackTarget = target;
                Animator.SetTrigger("target_selected");
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Grid.OnGridClicked -= new MapGrid.GridClickHandler(HandleGridClick);
    }
}