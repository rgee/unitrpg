using System.Collections.Generic;
using System.Linq;
using Grid;
using UnityEngine;

public class SelectingFightTarget : StateMachineBehaviour {
    private static readonly string ATTACK_SELECTION_NAME = "player_attack_range";
    private Animator Animator;
    private HashSet<Vector2> AttackableLocations;
    private MapGrid Grid;
    private BattleState State;
    private UnitManager UnitManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        State = CombatObjects.GetBattleState();
        Grid = CombatObjects.GetMap();
        UnitManager = CombatObjects.GetUnitManager();
        Animator = animator;

        var rangeFinder = new RangeFinder(Grid);
        AttackableLocations = rangeFinder.GetTilesInRange(State.SelectedGridPosition, 1)
                                         .Where(pos => UnitManager.GetUnitByPosition(pos) != null)
                                         .ToHashSet();

        MapHighlightManager.Instance.HighlightTiles(AttackableLocations,
            MapHighlightManager.HighlightLevel.PLAYER_ATTACK, ATTACK_SELECTION_NAME);

        Grid.OnGridClicked += HandleGridClick;
    }

    private void HandleGridClick(Vector2 loc) {
        if (AttackableLocations.Contains(loc)) {
            var target = UnitManager.GetUnitByPosition(loc);
            if (target != null) {
                State.AttackTarget = target;
                Animator.SetTrigger("target_selected");
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Grid.OnGridClicked -= HandleGridClick;
        MapHighlightManager.Instance.ClearHighlight(ATTACK_SELECTION_NAME);
    }
}