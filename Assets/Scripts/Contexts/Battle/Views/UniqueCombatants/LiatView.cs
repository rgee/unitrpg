using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Utilities;
using DG.Tweening;
using Models.Fighting;
using Models.Fighting.Effects;
using Models.Fighting.Execution;
using UnityEngine;

namespace Contexts.Battle.Views.UniqueCombatants {
    public class LiatView : CombatantView {
        private readonly Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _windupAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        private readonly Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _advanceAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        private readonly Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _farAdvanceAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        private tk2dSpriteAnimator _spriteAnimator;
        private CombatantAnimator _combatantAnimator;    
             

        protected override void Awake() {
            base.Awake();
            _spriteAnimator = GetComponent<tk2dSpriteAnimator>();
            _combatantAnimator = GetComponent<CombatantAnimator>();
            _windupAnimationClips[MathUtils.CardinalDirection.E] = FindClip("windup east");
            _windupAnimationClips[MathUtils.CardinalDirection.N] = FindClip("windup north");
            _windupAnimationClips[MathUtils.CardinalDirection.S] = FindClip("windup south");
            _windupAnimationClips[MathUtils.CardinalDirection.W] = FindClip("windup east");

            _advanceAnimationClips[MathUtils.CardinalDirection.W] = FindClip("advance east");
            _advanceAnimationClips[MathUtils.CardinalDirection.E] = FindClip("advance east");
            _advanceAnimationClips[MathUtils.CardinalDirection.S] = FindClip("advance south");
            _advanceAnimationClips[MathUtils.CardinalDirection.N] = FindClip("advance north");

            _farAdvanceAnimationClips[MathUtils.CardinalDirection.W] = FindClip("advance east long");
            _farAdvanceAnimationClips[MathUtils.CardinalDirection.E] = FindClip("advance east long");
            _farAdvanceAnimationClips[MathUtils.CardinalDirection.S] = FindClip("advance south long");
            _farAdvanceAnimationClips[MathUtils.CardinalDirection.N] = FindClip("advance north long");
        }

        public tk2dSpriteAnimationClip FindClip(string clipName) {
            var clip = _spriteAnimator.GetClipByName(clipName);
            if (clip == null) {
                throw new ArgumentException("Required clip \"" + clipName + "\" not found.");
            }

            return clip;
        }

        public override IEnumerator SpecialAttack(FightPhase phase, CombatantView receiverView, WeaponHitSeverity severity) {

            var receiver = phase.Receiver;
            var didAdvance = phase.Effects.ReceiverEffects.OfType<Advance>().Any();
            if (!didAdvance) {
                yield return StartCoroutine(Attack(receiver, severity));
                yield break;
            }

            var attacker = phase.Initiator;
            var distance = MathUtils.ManhattanDistance(attacker.Position, receiver.Position);
            var windupClip = distance > 1 ? _farAdvanceAnimationClips[Facing] : _windupAnimationClips[Facing];
            var advanceClip = _advanceAnimationClips[Facing];

            _combatantAnimator.enabled = false;
            yield return StartCoroutine(PlayClip(windupClip));

            var advanceClipDuration = ((1000/advanceClip.fps)*advanceClip.frames.Length/1000);
            var destination = receiverView.transform.position;
            transform
                .DOMove(destination, advanceClipDuration)
                .SetEase(Ease.OutCubic)
                .Play();
            yield return StartCoroutine(PlayClip(advanceClip));
            _combatantAnimator.enabled = true;
        }

        private IEnumerator PlayClip(tk2dSpriteAnimationClip clip) {
            _spriteAnimator.Play(clip);
            while (_spriteAnimator.IsPlaying(clip)) {
                yield return null;
            }
        }
    }
}