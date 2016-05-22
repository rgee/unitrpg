using System;
using System.Collections;
using System.Linq;
using Contexts.Battle.Utilities;
using DG.Tweening;
using Models.Fighting.Effects;
using Models.Fighting.Execution;

namespace Contexts.Battle.Views.UniqueCombatants {
    public class JanekView : CombatantView {
        public override IEnumerator SpecialAttack(FightPhase phase, MapDimensions dimensions, CombatantView receiverView, WeaponHitSeverity severity) {

            var receiver = phase.Receiver;
            var shoveEffects = phase.Effects.ReceiverEffects.OfType<Shove>().ToList();
            if (shoveEffects.Any()) {
                Action<WeaponHitConnection> hitConnectedCallback = null;
                hitConnectedCallback = hit => {
                    var destination = shoveEffects.First().GetDestination(receiver);
                    var worldDestination = dimensions.GetWorldPositionForGridPosition(destination);
                    var theirTransform = receiverView.transform;

                    theirTransform
                        .DOMove(worldDestination, 0.3f)
                        .SetEase(Ease.OutCubic)
                        .Play();
                    AttackConnectedSignal.RemoveListener(hitConnectedCallback);
                };

                AttackConnectedSignal.AddListener(hitConnectedCallback);
            }

            yield return StartCoroutine(Attack(receiver, severity));
        }
    }
}