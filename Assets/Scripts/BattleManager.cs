using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	private Animator battleStateManager;
	private Dictionary<int, BattleState> battleStateHash = new Dictionary<int, BattleState>();
	private BattleState currentBattleState;
    private Animator playerPhaseTextAnimator;

	public GameObject playerPhaseText;
    public Grid.UnitManager unitManager;
    public ActionMenuManager menuManager;
    public GridCameraController cameraController;

	public enum BattleState {
		Player_Phase_Intro,
		Enemy_Phase,
		Enemy_Phase_Intro,
		Select_Unit,
        Select_Fight_Action,
		Select_Action,
		Select_Target,
		Confirm_Forecast,
		Select_Move_Target,
		Battle_End,
		Forecasting,
		Performing_Action
	}

	public void StartPlayerPhase() {
		battleStateManager.SetTrigger("Player Phase Intro Animated");
        StartCoroutine(WaitAndUnlockControls());
	}

    public void StartActionSelect() {
        battleStateManager.SetTrigger("Unit Selected");
    }

    public void SelectBattleAction(string actionString) {
        switch (actionString) {
            case "move":
                SelectBattleAction(BattleAction.MOVE);
                break;
        }
    }

    public void CompletedMovement() {
        battleStateManager.SetTrigger("Movement Complete");
    }

    public void SelectBattleAction(BattleAction action) {
        if (action == BattleAction.MOVE) {
            battleStateManager.SetTrigger("Moving");
        } else if (action == BattleAction.FIGHT) {
            battleStateManager.SetTrigger("Acting");
        }
    }

    private IEnumerator WaitAndUnlockControls() {
        yield return new WaitForSeconds(0.5f);
        UnlockControls();
        yield return null;
    }

    private void UnlockControls() {
        cameraController.Unlock();
        unitManager.Unlock();
    }

    private void LockControls() {
        cameraController.Lock();
        unitManager.Lock();
    }

	void GetAnimationStates() {
		foreach (BattleState state in (BattleState[])System.Enum.GetValues(typeof(BattleState))) {
			int hash = Animator.StringToHash("Base Layer." + state.ToString());
			battleStateHash.Add(hash, state);
		}
	}

	void Start () {
		battleStateManager = GetComponent<Animator>();
		GetAnimationStates();
		playerPhaseTextAnimator = playerPhaseText.GetComponent<Animator>();
	
		RectTransform rect = playerPhaseText.GetComponent<RectTransform>();
		rect.anchorMax = new Vector2(0f, 0.5f);
		rect.anchorMin = new Vector2(0f, 0.5f);
		rect.anchoredPosition = new Vector3();

        LockControls();
        playerPhaseTextAnimator.SetTrigger("visible");
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
            LockControls();
			break;
		case BattleState.Select_Target:
			break;
		case BattleState.Confirm_Forecast:
			break;
        case BattleState.Select_Fight_Action:
            break;
		case BattleState.Select_Move_Target:
            UnlockControls();
            menuManager.HideCurrentMenu();
			break;
		default:
			break;
		}
	}
}
