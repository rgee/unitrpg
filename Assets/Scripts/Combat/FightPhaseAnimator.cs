using System;
using System.Collections;
using System.Linq;
using Grid;
using Models.Fighting.Effects;
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

            initUnit.CurrentAttackTarget = receiverUnit;

            var dodged = phase.Response == DefenderResponse.Dodge;
            if (dodged) {
                initUnit.Attacking = true;
                receiverUnit.Dodge();
            } else {
                if (phase.Effects.ReceiverEffects.OfType<Advance>().Any()) {
                    var unitGameObject = initUnit.gameObject;
                    var liatAnimator = unitGameObject.GetComponent<LiatAnimator>();
                    liatAnimator.AdvancingDestination = receiverUnit.transform.position;
                    liatAnimator.Advancing = true;
                    initUnit.Attacking = true;
                    yield return new WaitForSeconds(0.4f);
                    StartCoroutine(receiverUnit.GetComponent<UnitAnimator>().FadeToDeath(0.3f));
                }
            }

            yield return null;
        }
    }
}