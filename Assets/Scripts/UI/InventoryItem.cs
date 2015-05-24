using System;
using Models.Combat.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {
    public float SelectedAlpha = 0.5f;
    public Item Item;
    private float _originalAlpha;
    private Image _image;


    public delegate void UseHandler(Item item);
    public event UseHandler OnUse;

    public void Awake() {
        _image = GetComponent<Image>();
    }

    public void Use() {
        if (OnUse != null) {
            OnUse(Item);
        }
    }

    public void Select() {
        _originalAlpha = _image.color.a;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, SelectedAlpha);
    }

    public void Deselect() {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _originalAlpha);
    }
}
