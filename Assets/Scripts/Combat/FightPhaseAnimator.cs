using System;
using System.Collections;
using System.Linq;
using Grid;
using Models.Fighting.Effects;
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
                } else {
                    var shoveEffects = receiverEffects.OfType<Shove>();
                    if (shoveEffects.Any()) {
                        // TODO: Slide the target unit back a square.
                        Debug.Log("Awaiting hit connection callback to start knockback slide.");

                        Action hitConnectedCallback = null;
                        hitConnectedCallback = () => {
                            Debug.Log("Received hit confirmation. Starting knockback slide.");
                            var targetAnimator = receiverUnit.GetComponent<UnitAnimator>();
                            var destination = shoveEffects.First().GetDestination(phase.Receiver);
                            var worldDestination = MapGrid.Instance.GetWorldPosForGridPos(destination);
                            StartCoroutine(targetAnimator.SlideTo(worldDestination));
                            initUnit.OnHitConnected -= hitConnectedCallback;
                        };
                        initUnit.OnHitConnected += hitConnectedCallback;

                    }

                    initUnit.Attacking = true;
                }
            }

            yield return null;
        }
    }
}