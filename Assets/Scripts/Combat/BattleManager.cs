using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Grid;
using UnityEngine;

public class BattleManager : SceneEntryPoint {
    public bool StartImmediately;
    public List<GameObject> TriggerObjects;
    private readonly Dictionary<Vector2, List<CombatEvent>> _triggersByGridPosition = new Dictionary<Vector2, List<CombatEvent>>(); 

    public void Start() {
        var triggers = FindObjectsOfType<CombatEvent>();
        var grid = CombatObjects.GetMap();
        foreach (var trigger in triggers) {
            var triggerPosition = trigger.gameObject.transform.position;
            var position = grid.GridPositionForWorldPosition(triggerPosition);

            if (!_triggersByGridPosition.ContainsKey(position)) {
                _triggersByGridPosition[position] = new List<CombatEvent>();
            }

            _triggersByGridPosition[position].Add(trigger);
        }


        if (StartImmediately) {
            ApplicationEventBus.SceneStart.Dispatch();
        }
    }

    public IEnumerator RunTriggeredEvents(Vector2 destination) {
        if (!_triggersByGridPosition.ContainsKey(destination)) {
            yield return null;
        } else {
            var matchingTriggers = _triggersByGridPosition[destination];

            foreach (var trigger in matchingTriggers) {
                yield return StartCoroutine(trigger.Play());
            }
        }
    }

    public override void StartScene() {
        var stateMachine = GetComponent<Animator>();
        stateMachine.SetTrigger("battle_start");
    }
}
