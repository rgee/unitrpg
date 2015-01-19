using UnityEngine;
using System.Collections;

public class ActionMenuManager : MonoBehaviour {
    public GameObject TestMenu;
    private GameObject openMenu;

	public void ShowActionMenu(Grid.Unit unit) {
        GameObject menu = Instantiate(TestMenu) as GameObject;
        menu.transform.SetParent(unit.transform, true);
        menu.transform.localPosition = new Vector3(-16, 35, 0);

        openMenu = menu;
	}

    public void HideCurrentMenu() {
        Destroy(openMenu);
        openMenu = null;
    }
}
