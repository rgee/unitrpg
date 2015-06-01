﻿
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public Animator BattleStateMachine;
    private bool _isPaused;
    public void Start() {
      // Disable until I figure out TimeScale
      //CombatEventBus.Pauses.AddListener(TogglePause);  
    }

    private void TogglePause() {
        BattleStateMachine.SetBool("paused", !_isPaused);
        _isPaused = !_isPaused;
    }
}