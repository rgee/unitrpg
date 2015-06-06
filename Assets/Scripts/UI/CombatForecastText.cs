using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CombatForecastText : MonoBehaviour {
    public string Label;
    public int Value;
    public bool isDouble;

    private Text _labelText;
    private Text _valueText;
    private GameObject doubleText;

    public void Awake() {
        _labelText = transform.FindChild("Content/Label").GetComponent<Text>();
        _valueText = transform.FindChild("Content/Value").GetComponent<Text>();
        doubleText = transform.FindChild("DoubleText").gameObject;
    }

    public void Update() {
        _labelText.text = Label;
        _valueText.text = Value.ToString();
        doubleText.SetActive(isDouble);
    }
}
