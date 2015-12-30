using UnityEngine;

namespace Combat.Interactive.Rules {
    public class RequiredUnit : MonoBehaviour, ITileInteractivityRule {
        public bool CanBeUsed() {
            // TODO: Check that the currently selected unit is this one.
            return true;
        }
    }
}