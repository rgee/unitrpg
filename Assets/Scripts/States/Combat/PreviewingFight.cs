using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PreviewingFight : CancelableCombatState {

    private CombatForecaster Forecaster;
    private BattleState State;
    private Animator Animator;
	private FightResult PreviewResult;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Forecaster = GameObject.Find("Combat Forecast Manager").GetComponent<CombatForecaster>();
        State = GameObject.Find("BattleManager").GetComponent<BattleState>();
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
    }

    private void OnConfirm() {
		State.FightResult = PreviewResult;
        Animator.SetTrigger("attack_confirmed");
    }

    private void OnReject() {
        Animator.SetTrigger("attack_rejected");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Forecaster.OnConfirm -= new CombatForecaster.ForecastResponseHandler(OnConfirm);
        Forecaster.OnReject -= new CombatForecaster.ForecastResponseHandler(OnReject);
        Forecaster.HideCurrentForecast();
    }
}
