using System.Collections;
using UnityEngine;

public class FightExecutor : MonoBehaviour {
    private static readonly string FIGHT_START_TRIGGER = "fight_start";
    private bool resolved;
    private FightExcecutionState State;
    private Animator StateMachine;

    private void Awake() {
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