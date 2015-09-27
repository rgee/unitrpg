
using UnityEngine;

public class InventoryPopupManager : MonoBehaviour {
    public GameObject InventoryPopupPrefab;
    private GameObject _currentPopup;

    public InventoryPopup ShowInventory(Grid.Unit unit) {
        var unitObject = unit.gameObject;
        _currentPopup = Instantiate(InventoryPopupPrefab);

        var popupComponent = _currentPopup.GetComponent<InventoryPopup>();
        popupComponent.SetItems(unit.model.Inventory);

        _currentPopup.transform.SetParent(unitObject.transform, true);
        _currentPopup.transform.localPosition = new Vector3(-60, 17, 0);

        return popupComponent;
    }

    public void HideInventory() {
        if (_currentPopup != null) {
            Destroy(_currentPopup);
            _currentPopup = null;
        }
    }
}
