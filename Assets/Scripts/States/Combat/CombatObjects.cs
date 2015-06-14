using Grid;
using UnityEngine;

public static class CombatObjects {
    public static HealPreviewManager GetHealPreviewManager() {
        return GameObject.Find("BattleManager/HealPreviewManager").GetComponent<HealPreviewManager>();
    }

    public static MapGrid GetMap() {
        return GameObject.FindGameObjectWithTag("Map").GetComponent<MapGrid>();
    }

    public static Objective GetObjective() {
        return GameObject.FindGameObjectWithTag("Map").GetComponent<Objective>();
    }

    public static InventoryPopupManager GetInventoryPopupManager() {
        return GameObject.FindGameObjectWithTag("Inventory Popup Manager").GetComponent<InventoryPopupManager>();
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
        return GameObject.FindGameObjectWithTag("ActionMenuManager").GetComponent<ActionMenuManager>();
    }

    public static CombatForecaster GetCombatForecaster() {
        return GameObject.FindGameObjectWithTag("Forecaster").GetComponent<CombatForecaster>();
    }
}