using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PreviewingFight : CancelableCombatState {

    private CombatForecaster Forecaster;
    private BattleState State;
    private Animator Animator;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        Forecaster = GameObject.Find("Combat Forecast Manager").GetComponent<CombatForecaster>();
        State = GameObject.Find("BattleManager").GetComponent<BattleState>();
        Animator = animator;

        Forecaster.OnConfirm += new CombatForecaster.ForecastResponseHandler(OnConfirm);
        Forecaster.OnReject += new CombatForecaster.ForecastResponseHandler(OnReject);
        Forecaster.ShowAttackForecast(State.SelectedUnit.gameObject, State.AttackTarget.gameObject);
    }

    private void OnConfirm() {
        Animator.SetTrigger("attack_confirmed");
    }

    private void OnReject() {
        Animator.SetTrigger("attack_rejected");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Forecaster.OnConfirm -= new CombatForecaster.ForecastResponseHandler(OnConfirm);        Forecaster.OnReject -= new CombatForecaster.ForecastResponseHandler(OnReject);        Forecaster.HideCurrentForecast();    }}
