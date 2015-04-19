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
        State = CombatObjects.GetBattleState();
        Grid = CombatObjects.GetMap();
        UnitManager = CombatObjects.GetUnitManager();
        Animator = animator;

        RangeFinder rangeFinder = new RangeFinder(Grid);
        AttackableLocations = rangeFinder.GetTilesInRange(State.SelectedGridPosition, 1)
            .Where(pos => UnitManager.GetUnitByPosition(pos) != null)
            .ToHashSet();

        MapHighlightManager.Instance.HighlightTiles(AttackableLocations, MapHighlightManager.HiglightType.ATTACK);

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
        MapHighlightManager.Instance.ClearHighlight();
    }
}