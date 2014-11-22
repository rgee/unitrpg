using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	private Animator battleStateManager;
	private Dictionary<int, BattleState> battleStateHash = new Dictionary<int, BattleState>();
	private BattleState currentBattleState;

	public enum BattleState {
		Player_Phase_Intro,
		Enemy_Phase,
		Enemy_Phase_Intro,
		Select_Unit,
		Select_Action,
		Select_Target,
		Confirm_Forecast,
		Select_Move,
		Select_Move_Target
	}

	void GetAnimationStates() {
		foreach (BattleState state in (BattleState[])System.Enum.GetValues(typeof(BattleState))) {
			int hash = Animator.StringToHash("Base Layer." + state.ToString());
			battleStateHash.Add(hash, state);
		}
	}

	void Start () {
		battleStateManager = GetComponent<Animator>();
	}
	
	void Update () {
		currentBattleState = battleStateHash[battleStateManager.GetCurrentAnimatorStateInfo(0).nameHash];
		DispatchOnBatleState();
	}

	void OnGUI() {
		DispatchOnBatleState();
	}

	void DispatchOnBatleState() {

		switch (currentBattleState) {
		case BattleState.Player_Phase_Intro:
			break;
		case BattleState.Enemy_Phase:
			break;
		case BattleState.Enemy_Phase_Intro:
			break;
		case BattleState.Select_Unit:
			break;
		case BattleState.Select_Action:
			break;
		case BattleState.Select_Target:
			break;
		case BattleState.Confirm_Forecast:
			break;
		case BattleState.Select_Move:
			break;
		case BattleState.Select_Move_Target:
			break;
		default:
			break;
		}
	}
}
