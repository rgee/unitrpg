using UnityEngine;
using System.Collections;

public class InitialPlayerPhase : StateMachineBehaviour {

	private Grid.UnitManager UnitManager;
	private Animator Animator;
	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)	{
		Animator = animator;
		UnitManager = GameObject.Find("Unit Manager").GetComponent<Grid.UnitManager>();
		UnitManager.OnUnitClick += new Grid.UnitManager.UnitClickedEventHandler(OnUnitClicked);
	}

	private void OnUnitClicked(Grid.Unit unit) {
		string trigger = "enemy_selected";
		if (unit.friendly) {
			trigger = "friendly_selected";
		}

		Animator.SetTrigger(trigger);
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)	{
		UnitManager.OnUnitClick -= new Grid.UnitManager.UnitClickedEventHandler(OnUnitClicked);
	}
}
