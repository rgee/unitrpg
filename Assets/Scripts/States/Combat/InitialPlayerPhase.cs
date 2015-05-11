using System.Linq;
using Grid;
using Models.Combat;
using UnityEngine;

public class InitialPlayerPhase : StateMachineBehaviour {
    private Animator Animator;
    private BattleState BattleState;
    private UnitManager UnitManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Animator = animator;
        UnitManager = GameObject.Find("Unit Manager").GetComponent<UnitManager>();
        BattleState = GameObject.Find("BattleManager").GetComponent<BattleState>();

        MapHighlightManager.Instance.HoverSelectorEnabled = true;

        var friendlyUnits = UnitManager.GetFriendlies();

        var turnComplete = friendlyUnits.All(unit => BattleState.UnitActed(unit));

        if (turnComplete) {
            BattleState.Model.EndTurn(TurnControl.Friendly);
            animator.SetTrigger("actions_exhausted");
            BattleState.ResetTurnState();
        } else {
            UnitManager.OnUnitClick += OnUnitClicked;
        }
    }

    private void OnUnitClicked(Grid.Unit unit, Vector2 gridPosition, bool rightClick) {
        if (unit.friendly) {
            if (rightClick) {
                Animator.SetTrigger("info_selected");
            } else {
                BattleState.SelectedUnit = unit;
                BattleState.SelectedGridPosition = gridPosition;
                Animator.SetTrigger("friendly_selected");
            }
        } else if (!rightClick) {
            EnemyMoveRangeManager.Instance.ShowUnitMoveRange(unit);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        MapHighlightManager.Instance.HoverSelectorEnabled = false;
        UnitManager.OnUnitClick -= OnUnitClicked;
    }
}