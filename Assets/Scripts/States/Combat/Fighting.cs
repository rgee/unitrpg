using System.Collections;
using UnityEngine;

public class Fighting : StateMachineBehaviour {
    private CameraController camController;
    public GameObject FightExecutorPrefab;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var executorObject = Instantiate(FightExecutorPrefab);
        var executor = executorObject.GetComponent<FightExecutor>();
        var state = CombatObjects.GetBattleState();
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
        Destroy(executor.gameObject);

        if (state.SelectedUnit == null) {
            stateMachine.SetTrigger("friendly_dies");
        } else {
            stateMachine.SetTrigger("fight_completed");
        }
    }
}