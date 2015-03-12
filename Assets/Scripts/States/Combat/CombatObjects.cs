using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CombatObjects {
    public static BattleState GetBattleState() {
        return GameObject.Find("BattleManager").GetComponent<BattleState>();
    }
}