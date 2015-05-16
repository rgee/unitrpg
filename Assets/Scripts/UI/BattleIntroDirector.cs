using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BattleIntroDirector : MonoBehaviour {
    public event Action OnIntroComplete;

    public void StartIntro() {
        if (OnIntroComplete != null) {
            OnIntroComplete();
        }
    }
}