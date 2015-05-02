using UnityEngine;

public class FightExcecutionState : MonoBehaviour {
    public GameObject Attacker;
    public bool Complete;
    public GameObject Defender;
    public FightResult Result;

    public void SetNewFight(GameObject attacker, GameObject defender, FightResult result) {
        Attacker = attacker;
        Defender = defender;
        Result = result;
        Complete = false;
    }
}