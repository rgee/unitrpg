using UnityEngine;
using System;
using System.Collections;

public class ActionMenuManager : MonoBehaviour {

	public delegate void ActionSelectedHandler(BattleAction action);
	public event ActionSelectedHandler OnActionSelected;

    public GameObject TestMenu;
    private GameObject openMenu;

	public void ShowActionMenu(Grid.Unit unit) {
        GameObject menu = TestMenu;

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
		BattleAction action = (BattleAction)Enum.Parse(typeof(BattleAction), name);
		if (OnActionSelected != null) {
			OnActionSelected(action);
		}
	}
}
