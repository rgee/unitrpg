using System;
using UnityEngine;
using System.Collections;

public class HealPreview : MonoBehaviour {

    public event Action OnConfirm;

    void Start() {

    }

    void Update() {

    }

    public void Confirm() {
        if (OnConfirm == null) {
            return;
        }

        OnConfirm();
    }
}
