using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PreviewingFight : CancelableCombatState {

    private CombatForecaster Forecaster;
    private BattleState State;
    private Animator Animator;
	private FightResult PreviewResult;
    private GridCameraController Camera;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Camera = CombatObjects.GetCameraController();
        Forecaster = GameObject.Find("Combat Forecast Manager").GetComponent<CombatForecaster>();
        State = CombatObjects.GetBattleState();
        Animator = animator;

		Fight fight = new Fight(
			new Participants(State.SelectedUnit.GetComponent<Grid.Unit>().model, State.AttackTarget.GetComponent<Grid.Unit>().model),
			AttackType.BASIC,
			new DefaultFightResolution()
		);

		PreviewResult = fight.SimulateFight();

        Forecaster.OnConfirm += new CombatForecaster.ForecastResponseHandler(OnConfirm);
        Forecaster.OnReject += new CombatForecaster.ForecastResponseHandler(OnReject);
        Forecaster.ShowAttackForecast(PreviewResult, State.SelectedUnit.gameObject, State.AttackTarget.gameObject);

        Grid.Unit attacker = State.SelectedUnit;
        Grid.Unit defender = State.AttackTarget;

        MathUtils.CardinalDirection attackerDirection = MathUtils.DirectionTo(attacker.gridPosition, defender.gridPosition);
        MathUtils.CardinalDirection defenderDirection = attackerDirection.GetOpposite();

        attacker.PrepareForCombat(attackerDirection);
        defender.PrepareForCombat(defenderDirection);

        Camera.DisbleGridSelector();
    }

    private void OnConfirm() {
		State.FightResult = PreviewResult;
        Animator.SetTrigger("attack_confirmed");
    }

    private void OnReject() {
        Animator.SetTrigger("attack_rejected");
        State.SelectedUnit.ReturnToRest();
        State.AttackTarget.ReturnToRest();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Forecaster.OnConfirm -= new CombatForecaster.ForecastResponseHandler(OnConfirm);
        Forecaster.OnReject -= new CombatForecaster.ForecastResponseHandler(OnReject);
        Forecaster.HideCurrentForecast();
        Camera.EnableGridSelector();
    }
}
