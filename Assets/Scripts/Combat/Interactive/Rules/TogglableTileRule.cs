using UnityEngine;

namespace Combat.Interactive.Rules {
    public class TogglableTileRule : MonoBehaviour, ITileInteractivityRule {
        public bool Enabled = true;

        public bool CanBeUsed() {
            return Enabled;
        }
    }
}