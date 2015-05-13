using Grid;
using UnityEngine;

public static class CombatObjects {
    public static MapGrid GetMap() {
        return GameObject.FindGameObjectWithTag("Map").GetComponent<MapGrid>();
    }

    public static InventoryPopupManager GetInventoryPopupManager() {
        return GameObject.Find("InventoryPopupManager").GetComponent<InventoryPopupManager>();
    }

    public static AIManager GetAIManager() {
        return GameObject.Find("AI Manager").GetComponent<AIManager>();
    }

    public static BattleState GetBattleState() {
        return GameObject.Find("BattleManager").GetComponent<BattleState>();
    }

    public static UnitManager GetUnitManager() {
        return GameObject.Find("Unit Manager").GetComponent<UnitManager>();
    }

    public static GridCameraController GetCameraController() {
        return GameObject.Find("Grid Camera/Main Camera").GetComponent<GridCameraController>();
    }

    public static ActionMenuManager GetActionMenuManager() {
        return GameObject.Find("ActionMenuManager").GetComponent<ActionMenuManager>();
    }
}