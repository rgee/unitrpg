using UnityEngine;

public static class FightExecutionObjects {
    public static FightExecutor GetExecutor() {
        return GameObject.FindGameObjectWithTag("Fight Executor").GetComponent<FightExecutor>();
    }

    public static FightExcecutionState GetState() {
        return GetExecutor().gameObject.GetComponent<FightExcecutionState>();
    }

    public static ScreenShaker GetScreenShaker() {
        return CombatObjects.GetCameraController().GetComponent<ScreenShaker>();
    }
}