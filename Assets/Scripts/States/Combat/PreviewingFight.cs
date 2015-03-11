using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PreviewingFight : StateMachineBehaviour {

    private CombatForecaster Forecaster;
    private BattleState State;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Forecaster = GameObject.Find("Combat Forecast Manager").GetComponent<CombatForecaster>();
        State = GameObject.Find("BattleManager").GetComponent<BattleState>();

        Forecaster.ShowAttackForecast(State.SelectedUnit.gameObject, State.AttackTarget.gameObject);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Forecaster.HideCurrentForecast();    }}
