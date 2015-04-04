using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class CombatObjects {
    public static MapGrid GetMap() {
        return GameObject.FindGameObjectWithTag("Map").GetComponent<MapGrid>();
    }

    public static BattleState GetBattleState() {
        return GameObject.Find("BattleManager").GetComponent<BattleState>();
    }

    public static Grid.UnitManager GetUnitManager() {
        return GameObject.Find("Unit Manager").GetComponent<Grid.UnitManager>();
    }

    public static GridCameraController GetCameraController() {
        return GameObject.Find("Grid Camera/Main Camera").GetComponent<GridCameraController>();
    }
}