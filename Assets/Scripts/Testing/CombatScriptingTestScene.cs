using Grid;
using UnityEngine;

namespace Assets.Testing {
    public class CombatScriptingTestScene : MonoBehaviour {
        private UnitManager _unitManager;

        void Awake() {
            _unitManager = CombatObjects.GetUnitManager();
        }
    }
}