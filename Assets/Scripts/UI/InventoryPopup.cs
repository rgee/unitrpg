using System.Collections.Generic;
using Models.Combat.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour {
    public GameObject ItemPrefab;
    private GameObject _itemsContainer;

    public void Awake() {
        _itemsContainer = transform.FindChild("Panel/Stack/Items").gameObject;
    }

    public void SetItems(List<Item> items) {
        Transform containerTransform = _itemsContainer.transform;
        foreach (Transform child in containerTransform) {
            GameObject childObject = child.gameObject;
            Destroy(childObject);
        }

        foreach (Item item in items) {
            GameObject itemObject = Instantiate(ItemPrefab);
            itemObject.transform.SetParent(containerTransform);

            Text textComponent = itemObject.GetComponent<Text>();
            textComponent.text = item.Name;
        }
    }
}
