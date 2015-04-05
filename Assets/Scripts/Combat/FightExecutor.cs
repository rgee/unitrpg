using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FightExecutor : MonoBehaviour {
    private static string FIGHT_START_TRIGGER = "fight_start";

    private FightExcecutionState State;
    private Animator StateMachine;
    private bool resolved;

    void Awake() {
        State = GetComponent<FightExcecutionState>();
        StateMachine = GetComponent<Animator>();
    }

    public IEnumerator RunFight(GameObject attacker, GameObject defender, FightResult result) {
        State.SetNewFight(attacker, defender, result);
        StateMachine.SetTrigger(FIGHT_START_TRIGGER);
        while (!State.Complete) {
            yield return null;
        }
    }
}