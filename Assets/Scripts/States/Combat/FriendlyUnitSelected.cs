using UnityEngine;
using System.Collections;

public class FriendlyUnitSelected : StateMachineBehaviour {

	private BattleState BattleState;
	private ActionMenuManager MenuManager;
	private Animator Animator;

	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Animator = animator;
		BattleState = GameObject.Find("BattleManager").GetComponent<BattleState>();
		MenuManager = GameObject.Find("ActionMenuManager").GetComponent<ActionMenuManager>();

		MenuManager.OnActionSelected += new ActionMenuManager.ActionSelectedHandler(HandleAction);
		MenuManager.ShowActionMenu(BattleState.SelectedUnit);
	}

	private void HandleAction(BattleAction action) {
		switch (action) {
		case BattleAction.FIGHT:
			Animator.SetTrigger("fight_selected");
			break;
		case BattleAction.MOVE:
			Animator.SetTrigger("move_selected");
			break;
		}
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		MenuManager.OnActionSelected -= new ActionMenuManager.ActionSelectedHandler(HandleAction);
		MenuManager.HideCurrentMenu();
	}
}
