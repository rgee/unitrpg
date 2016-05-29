using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Utilities;
using Contexts.Battle.Views;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using UnityEngine;

namespace Combat {
    public class FightPhaseAnimator : MonoBehaviour {
        private readonly HashSet<SkillType> _specialSkills = new HashSet<SkillType> {
           SkillType.Kinesis,
           SkillType.Knockback,
           SkillType.Advance,
           SkillType.Strafe 
        };

        public IEnumerator Animate(FightPhase phase, CombatantView initiator, CombatantView receiver, MapDimensions dimensions) {
            if (phase.Response == DefenderResponse.Dodge) {
                StartCoroutine(receiver.Dodge());
            }

            var severity = phase.Effects.Severity;

            if (_specialSkills.Contains(phase.Skill)) {
                yield return StartCoroutine(initiator.SpecialAttack(phase, dimensions, receiver, severity));
            } else {
                yield return StartCoroutine(initiator.Attack(phase.Receiver, severity));
            }

            if (phase.ReceverDies) {
                receiver.Die();
            }
        }
    }
}