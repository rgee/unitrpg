
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Grid;
using UnityEngine;

public class BattleManager : SceneEntryPoint {
    public bool StartImmediately;
    public List<GameObject> TriggerObjects;
    private List<CombatEvent> _triggers = new List<CombatEvent>();
    private List<CombatEvent> _activeTriggers = new List<CombatEvent>(); 

    public void Start() {
        foreach (var trigger in TriggerObjects) {
            var createdTrigger = Instantiate(trigger);
            var component = createdTrigger.GetComponent<CombatEvent>();
            _triggers.Add(component);
        }

        CombatEventBus.Moves.AddListener(CheckTriggers);

        if (StartImmediately) {
            ApplicationEventBus.SceneStart.Dispatch();
        }
    }

    public IEnumerator RunTriggeredEvents(Action onComplete) {
        while (_activeTriggers.Any()) {
            var trigger = _activeTriggers[0];
            yield return StartCoroutine(trigger.Play());
            _activeTriggers.Remove(trigger);
        }

        onComplete();
    }

    private void CheckTriggers(Grid.Unit unit, Vector2 destination) {
        var matchingTriggers = _triggers.Where(trigger => trigger.Location == destination).ToList();
        if (matchingTriggers.Any()) {
            _activeTriggers.AddRange(matchingTriggers);
        }
    }

    public override void StartScene() {
        var stateMachine = GetComponent<Animator>();
        stateMachine.SetTrigger("battle_start");
    }
}
