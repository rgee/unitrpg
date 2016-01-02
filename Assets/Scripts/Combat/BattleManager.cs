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

    public void Start() {
        foreach (var trigger in TriggerObjects) {
            var createdTrigger = Instantiate(trigger);
            var component = createdTrigger.GetComponent<CombatEvent>();
            _triggers.Add(component);
        }

        if (StartImmediately) {
            ApplicationEventBus.SceneStart.Dispatch();
        }
    }

    public IEnumerator RunTriggeredEvents(Vector2 destination) {
        var matchingTriggers = _triggers.Where(trigger => trigger.Locations.Contains(destination)).ToList();

        Debug.Log("Running triggered events");
        foreach (var trigger in matchingTriggers) {
            yield return StartCoroutine(trigger.Play());
        }
    }

    public override void StartScene() {
        var stateMachine = GetComponent<Animator>();
        stateMachine.SetTrigger("battle_start");
    }
}
