using System.Collections;
using Grid;
using Models.Fighting.Execution;
using UnityEngine;

namespace Combat {
    public class FightPhaseAnimator : MonoBehaviour {
        private UnitManager _unitManager;

        void Awake() {
            _unitManager = CombatObjects.GetUnitManager();
        }

        public IEnumerator Animate(FightPhase phase) {
            Debug.Log("Testing fight");
            yield return null;
        }
    }
}