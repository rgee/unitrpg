using UnityEngine;

public class ActionMenuController : MonoBehaviour {
    public void TriggerAction(string action) {
        CombatObjects.GetActionMenuManager().SelectAction(action);
    }
}
