using UnityEngine;
using System.Collections;

public class ActionMenuManager : MonoBehaviour {
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
}
