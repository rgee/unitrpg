using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class FightExecutionObjects {
    public static FightExcecutionState GetState() {
        return GameObject.FindGameObjectWithTag("Fight Executor").GetComponent<FightExcecutionState>();
    }

    public static ScreenShaker GetScreenShaker() {
        return CombatObjects.GetCameraController().GetComponent<ScreenShaker>();
    }
}
