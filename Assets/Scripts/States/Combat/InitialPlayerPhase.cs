using UnityEngine;
using System.Collections;

public class InitialPlayerPhase : StateMachineBehaviour {

	private Grid.UnitManager UnitManager;
	public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)	{
		UnitManager = GameObject.Find("Unit Manager").GetComponent<Grid.UnitManager>();
		UnitManager.OnUnitClick += new Grid.UnitManager.UnitClickedEventHandler(OnUnitClicked);
	}

	private void OnUnitClicked(Grid.Unit unit) {
		Debug.Log ("Unit clicked");
	}

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)	{
		UnitManager.OnUnitClick -= new Grid.UnitManager.UnitClickedEventHandler(OnUnitClicked);
	}
}
