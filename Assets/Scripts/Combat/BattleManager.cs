
using UnityEngine;

public class BattleManager : SceneEntryPoint {
    public override void StartScene() {
        var stateMachine = GetComponent<Animator>();
        stateMachine.SetTrigger("battle_start");
    }
}
