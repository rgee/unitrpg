using System;
using UnityEngine;

public class ActionMenuManager : MonoBehaviour {
    public delegate void ActionSelectedHandler(BattleAction action);

    private GameObject openMenu;
    public GameObject TestMenu;
    public event ActionSelectedHandler OnActionSelected;

    public void ShowActionMenu(Grid.Unit unit) {
        var menu = TestMenu;

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