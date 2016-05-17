using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthBarManager : MonoBehaviour {
    public GameObject HealthBarPrefab;
    public Vector2 BarOffset;

    private Dictionary<GameObject, GameObject> _healthBars = new Dictionary<GameObject, GameObject>();

    public void Start() {
        CombatEventBus.HealthBarToggles.AddListener(this.ToggleHealthBars);       
    }

    private void ToggleHealthBars() {
        if (_healthBars.Any()) {
            HideHealthBars();
        } else {
            ShowHealthBars();
        }
    }

    public void Update() {
        foreach (var entry in _healthBars) {
            var unit = entry.Key.GetComponent<Grid.Unit>().model;
            var healthPct = (float)unit.Health/unit.Character.MaxHealth;
            entry.Value.GetComponent<HealthBar>().HealthPct = healthPct*100;
            AlignToUnit(entry.Key, entry.Value);
        }
    }

    public void ShowHealthBars() {
        var unitManager = CombatObjects.GetUnitManager();
        var units = from unit in unitManager.GetAllUnits()
                    select unit.gameObject;
        foreach (var unit in units) {
            AddHealthBar(unit);
        }
    }

    public void HideHealthBars() {
        foreach (var bar in _healthBars.Values) {
            Destroy(bar);
        }

        _healthBars.Clear();
    }

    private void AlignToUnit(GameObject unit, GameObject bar) {
        var unitPosition = unit.transform.position;
        bar.GetComponent<RectTransform>().anchoredPosition = unitPosition + new Vector3(BarOffset.x, BarOffset.y); 
    }

    private void AddHealthBar(GameObject unit) {
        var bar = Instantiate(HealthBarPrefab);
        AlignToUnit(unit, bar);
        _healthBars[unit] = bar;
    }
}
