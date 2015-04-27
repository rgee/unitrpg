using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InitialPlayerPhase : StateMachineBehaviour {
	private Grid.UnitManager UnitManager;
	private Animator Animator;
	private BattleState BattleState;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)	{
		Animator = animator;
		UnitManager = GameObject.Find("Unit Manager").GetComponent<Grid.UnitManager>();
		BattleState = GameObject.Find("BattleManager").GetComponent<BattleState>();

        MapHighlightManager.Instance.HoverSelectorEnabled = true;

        List<Grid.Unit> friendlyUnits = UnitManager.GetFriendlies();

        bool turnComplete = friendlyUnits.All(unit => {
			return BattleState.UnitActed(unit);
        });

        if (turnComplete) {
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

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)	{
        MapHighlightManager.Instance.HoverSelectorEnabled = false;
	    UnitManager.OnUnitClick -= OnUnitClicked;
	}
}
