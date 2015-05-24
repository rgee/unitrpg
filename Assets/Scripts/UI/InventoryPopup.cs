using System;
using System.Collections.Generic;
using Models.Combat.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour {
    public GameObject ItemPrefab;
    private GameObject _itemsContainer;

    public delegate void UseHandler(Item item);
    public event UseHandler OnItemUse;

    public void Awake() {
        _itemsContainer = transform.FindChild("Stack/Items").gameObject;
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

            GameObject itemLabelObject = itemObject.transform.FindChild("Label").gameObject;
            Text textComponent = itemLabelObject.GetComponent<Text>();
            textComponent.text = item.Name;

            var itemComponent = itemObject.GetComponent<InventoryItem>();
            itemComponent.Item = item;
            itemComponent.OnUse += DispatchUse;
        }
    }

    private void DispatchUse(Item item) {
        if (OnItemUse != null) {
            OnItemUse(item);
        } 
    }
}
