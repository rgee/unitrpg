
using UnityEngine;

public class BattleManager : SceneEntryPoint {
    public bool StartImmediately;

    public void Start() {
        if (StartImmediately) {
            ApplicationEventBus.SceneStart.Dispatch();
        }
    }

    public override void StartScene() {
        var stateMachine = GetComponent<Animator>();
        stateMachine.SetTrigger("battle_start");
    }
}
