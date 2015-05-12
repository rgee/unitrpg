using System;
using System.Collections.Generic;
using System.Linq;
using Models.Combat;
using UnityEngine;

public class ActionMenuManager : MonoBehaviour {
    public delegate void ActionSelectedHandler(BattleAction action);

    private GameObject openMenu;
    public GameObject TestMenu;
    public event ActionSelectedHandler OnActionSelected;

    // For when there are enemies nearby, but no friendlies
    public GameObject MoveFightFull;

    // For when there are no enemies or friendly units around and the
    // unit can still move.
    public GameObject MoveBraceItem;

    private Dictionary<CombatAction, GameObject> PrefabsByActions = new Dictionary<CombatAction, GameObject>();

    public void Start() {
        PrefabsByActions.Add(
            CombatAction.Fight | CombatAction.Wait | CombatAction.Move | CombatAction.Brace | CombatAction.Item,
            MoveFightFull
        );

        PrefabsByActions.Add(
            CombatAction.Move | CombatAction.Wait | CombatAction.Brace | CombatAction.Item,
            MoveBraceItem
        );
    }

    public void ShowActionMenu(Grid.Unit unit) {
        var battle = CombatObjects.GetBattleState().Model;

        var availableActionEnums = battle.GetAvailableActions(unit.model);

        var availableActions = availableActionEnums
            .Aggregate((value, next) => value | next);

        var menu = TestMenu;
        if (PrefabsByActions.ContainsKey(availableActions)) {
            Debug.Log("Matched menu from bit mask");
        }

        menu.SetActive(true);
        menu.transform.SetParent(unit.transform, true);
        menu.transform.localPosition = new Vector3(-16, 35, 0);

        openMenu = menu;
    }

    public void HideCurrentMenu() {
        if (openMenu != null) {
            openMenu.SetActive(false);
            openMenu = null;
        }
    }

    public void SelectAction(string name) {
        var action = (BattleAction) Enum.Parse(typeof (BattleAction), name);
        if (OnActionSelected != null) {
            OnActionSelected(action);
        }
    }
}