using System;
using System.Collections.Generic;
using System.Linq;
using strange.extensions.signal.impl;
using UnityEngine;

public class HotkeyDispatcher : MonoBehaviour {
    private readonly Dictionary<KeyCode, Signal> dispatchTable = new Dictionary<KeyCode, Signal>();

    public void Start() {
        dispatchTable[KeyCode.H] = CombatEventBus.HealthBarToggles;
        dispatchTable[KeyCode.Escape] = CombatEventBus.Pauses;
    }

    public void Update() {
        var activeSignals = from KeyCode keyCode in Enum.GetValues(typeof(KeyCode)) 
                            where dispatchTable.ContainsKey(keyCode) && Input.GetKeyDown(keyCode)
                            select dispatchTable[keyCode];

        foreach (var signal in activeSignals) {
            signal.Dispatch();
        }
    }
}
