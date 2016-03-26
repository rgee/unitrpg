using System;
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
            var initUnit = _unitManager.GetUnitByName(phase.Initiator.Name);
            var receiverUnit = _unitManager.GetUnitByName(phase.Receiver.Name);

            initUnit.Attacking = true;

            var dodged = phase.Response == DefenderResponse.Dodge;
            if (dodged) {
                receiverUnit.Dodge();
            }

            yield return null;
        }
    }
}