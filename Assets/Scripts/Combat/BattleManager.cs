
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class BattleManager : SceneEntryPoint {
    public bool StartImmediately;
    public List<GameObject> TriggerObjects;
    private List<Trigger> _triggers = new List<Trigger>();

    public void Start() {
        foreach (var trigger in TriggerObjects) {
            _triggers.Add(trigger.GetComponent<Trigger>());
        }

        if (StartImmediately) {
            ApplicationEventBus.SceneStart.Dispatch();
        }
    }

    public override void StartScene() {
        var stateMachine = GetComponent<Animator>();
        stateMachine.SetTrigger("battle_start");
    }
}
