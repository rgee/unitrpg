using System.Collections.Generic;
using UnityEngine;

public class MovementDialog : MonoBehaviour {
    private readonly List<GameObject> Pips = new List<GameObject>();
    public GameObject PipPrefab;
    public int TotalMoves;
    public int UsedMoves;

    private void Start() {
        var pipParent = transform.FindChild("Panel/Pips").gameObject;
        for (var i = 0; i < TotalMoves; i++) {
            var pip = Instantiate(PipPrefab);
            Pips.Add(pip);

            pip.transform.parent = pipParent.transform;
        }
    }

    private void Update() {
        var enabledPips = TotalMoves - UsedMoves;
        var numEnabled = 0;

        foreach (var pip in Pips) {
            var comp = pip.GetComponent<Pip>();
            if (numEnabled < enabledPips) {
                comp.IsEnabled = true;
                numEnabled++;
            } else {
                comp.IsEnabled = false;
            }
        }
    }
}