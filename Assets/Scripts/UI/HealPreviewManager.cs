using System;
using UnityEngine;
using System.Collections;

public class HealPreviewManager : MonoBehaviour {
    public GameObject PreviewPrefab;

    private GameObject _openPreview;
    private BattleState _state;
    // Use this for initialization
    void Start() {
        _state = CombatObjects.GetBattleState();
    }

    public HealPreview ShowHealPreview() {
        if (_state.SelectedItem == null) {
            throw new ArgumentException("There must be an item selected to show a heal preview.");
        }

        var preview = Instantiate(PreviewPrefab);
        var component = preview.GetComponent<HealPreview>();

        component.SetPreview(_state.SelectedItem, _state.SelectedUnit);
        var unitObject = _state.SelectedUnit.gameObject;
        AlignToUnit(unitObject, preview);

        _openPreview = preview;

        return component;
    }

    public void HideHealPreview() {
        if (_openPreview == null) {
            return;
        }

        Destroy(_openPreview);
        _openPreview = null;
    }

    private static void AlignToUnit(GameObject unit, GameObject bar) {
        var unitPosition = unit.transform.position;
        bar.GetComponent<RectTransform>().anchoredPosition = unitPosition;
    }
}
