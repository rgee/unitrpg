﻿using Combat.Interactables;
using Grid;
using UI.ActionMenu;
using UnityEngine;

public static class CombatObjects {
    public static BattleManager GetBattleManager() {
        return GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
    }

    public static HealPreviewManager GetHealPreviewManager() {
        return GameObject.Find("BattleManager/HealPreviewManager").GetComponent<HealPreviewManager>();
    }

    public static InteractiveSquareManager GetInteractiveSquareManager() {
        return GameObject.FindGameObjectWithTag("InteractiveSquareManager").GetComponent<InteractiveSquareManager>();
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

    public static GameObject GetIntroSequence() {
        return GameObject.FindGameObjectWithTag("Intro Sequence");
    }

    public static GameObject GetCamera() {
        return GameObject.FindGameObjectWithTag("MainCamera");
    }
    public static GridCameraController GetCameraController() {
        return GetCamera().GetComponent<GridCameraController>();
    }

    public static ActionMenu GetActionMenu() {
        return GameObject.FindGameObjectWithTag("Action Menu").GetComponent<ActionMenu>();
    }

    public static CombatForecaster GetCombatForecaster() {
        return GameObject.FindGameObjectWithTag("Forecaster").GetComponent<CombatForecaster>();
    }
}