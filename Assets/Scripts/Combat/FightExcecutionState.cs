using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FightExcecutionState : MonoBehaviour {
    public GameObject Attacker;
    public GameObject Defender;
    public FightResult Result;
    public bool Complete;

    public void SetNewFight(GameObject attacker, GameObject defender, FightResult result) {
        this.Attacker = attacker;
        this.Defender = defender;
        this.Result = result;
        this.Complete = false;
    }
}