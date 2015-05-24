using System;
using UnityEngine;
using System.Collections;
using Models.Combat.Inventory;
using UnityEngine.UI;

public class HealPreview : MonoBehaviour {

    public Item item;
    public Grid.Unit unit;
    public event Action OnConfirm;

    void Start() {
    }

    public void SetPreview(Item item, Grid.Unit unit) {
        var currentHealth = unit.model.Health;
        var eventualHealth = Math.Min(unit.model.Character.MaxHealth, currentHealth + item.HealAmount);
        var fromValue = transform.FindChild("From Health Panel/Value");
        var toValue = transform.FindChild("To Health Panel/Value");

        fromValue.GetComponent<Text>().text = currentHealth.ToString();
        toValue.GetComponent<Text>().text = eventualHealth.ToString();
    }

    public void Confirm() {
        if (OnConfirm == null) {
            return;
        }

        OnConfirm();
    }
}
