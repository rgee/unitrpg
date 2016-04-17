using System;
using System.Collections;
using System.Linq;
using Grid;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using UnityEngine;
using Advance = Models.Fighting.Effects.Advance;

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
            }
            else {
                var receiverEffects = phase.Effects.ReceiverEffects;
                if (receiverEffects.OfType<Advance>().Any()) {
                    var unitGameObject = initUnit.gameObject;
                    var liatAnimator = unitGameObject.GetComponent<LiatAnimator>();
                    liatAnimator.Advance(receiverUnit.transform.position);
                    yield return new WaitForSeconds(0.4f);
                    StartCoroutine(receiverUnit.GetComponent<UnitAnimator>().FadeToDeath(0.3f));
                } else if (receiverEffects.OfType<Knockback>().Any()) {
                    // TODO: Activate the Janek knockback animation
                    // TODO: Slide the target unit back a square.
                } else {
                    initUnit.Attacking = true;
                }
            }

            yield return null;
        }
    }
}