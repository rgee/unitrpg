using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
	private Animator battleStateManager;
	private Dictionary<int, BattleState> battleStateHash = new Dictionary<int, BattleState>();
	private BattleState? currentBattleState = null;
    private Animator playerPhaseTextAnimator;

    private int turn = 0;

	public GameObject playerPhaseText;
    public MapGrid map;
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
            case "fight":
                SelectBattleAction(BattleAction.FIGHT);
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

            // TODO: Figure out why this extra state is here;
            battleStateManager.SetTrigger("Action Confirmed");
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
	}
	
	void Update () {
        BattleState nextState = battleStateHash[battleStateManager.GetCurrentAnimatorStateInfo(0).nameHash];
        bool stateChange = !currentBattleState.HasValue || nextState != currentBattleState.Value;
        currentBattleState = nextState;

        if (stateChange) {
            DispatchOnBatleState();
        }
	}

	void DispatchOnBatleState() {

		switch (currentBattleState.Value) {
		case BattleState.Player_Phase_Intro:
            LockControls();
            playerPhaseTextAnimator.SetTrigger("visible");
			break;
		case BattleState.Enemy_Phase:
            turn++;
            unitManager.ResetMovedUnits(true);
			break;
		case BattleState.Enemy_Phase_Intro:
			break;
		case BattleState.Select_Unit:
            if (!unitManager.UnitsRemainingToMove()) {
                battleStateManager.SetTrigger("All Units Acted");
                turn++;
                unitManager.ResetMovedUnits(false);
            }
			break;
		case BattleState.Select_Action:
            LockControls();
			break;
		case BattleState.Select_Target:
            UnlockControls();
            menuManager.HideCurrentMenu();
            SelectAttackableArea(); 
			break;
		case BattleState.Confirm_Forecast:
			break;
        case BattleState.Select_Fight_Action:
            break;
		case BattleState.Select_Move_Target:
            UnlockControls();
            menuManager.HideCurrentMenu();
            SelectWalkableArea();
			break;
		default:
			break;
		}
	}

    private void SelectAttackableArea() {
        Vector2? maybePosition = unitManager.GetSelectedGridPosition();
        if (maybePosition.HasValue) {
            HashSet<Vector2> positions = map.GetWalkableTilesInRange(maybePosition.Value, 1);
            map.SelectTiles(positions, Color.red);
        }
    }

    private void SelectWalkableArea() {
        Vector2? maybePosition = unitManager.GetSelectedGridPosition();
        if (maybePosition.HasValue) {
            HashSet<Vector2> positions = map.GetWalkableTilesInRange(maybePosition.Value, 4);
            map.SelectTiles(positions, Color.blue);
        }
    }
}
