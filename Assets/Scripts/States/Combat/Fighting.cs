using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using UnityEngine;

public class Fighting : StateMachineBehaviour {
    public GameObject FightExecutorPrefab;
    private CameraController camController;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameObject executorObject = Instantiate(FightExecutorPrefab) as GameObject;
        FightExecutor executor = executorObject.GetComponent<FightExecutor>();
        BattleState state = CombatObjects.GetBattleState();
        camController = CombatObjects.GetCameraController();

        camController.Lock();
        executor.StartCoroutine(Execute(executor, state, animator));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        camController.Unlock();
    }

    private IEnumerator Execute(FightExecutor executor, BattleState state, Animator stateMachine) {
        yield return executor.StartCoroutine(executor.RunFight(
            state.SelectedUnit.gameObject,
            state.AttackTarget.gameObject,
            state.FightResult
        ));
        Destroy(executor);
        stateMachine.SetTrigger("fight_completed");
    }
}
