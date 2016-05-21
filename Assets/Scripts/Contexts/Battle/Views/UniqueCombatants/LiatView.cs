using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Models.Fighting;
using Models.Fighting.Effects;
using UnityEngine;

namespace Contexts.Battle.Views.UniqueCombatants {
    public class LiatView : CombatantView {
        private Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _windupAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        private readonly Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _advanceAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        private tk2dSpriteAnimator _animator;

        new void Awake() {
            base.Awake();
            _animator = GetComponent<tk2dSpriteAnimator>();
            _windupAnimationClips[MathUtils.CardinalDirection.E] = FindClip("windup east");
            _windupAnimationClips[MathUtils.CardinalDirection.N] = FindClip("windup north");
            _windupAnimationClips[MathUtils.CardinalDirection.S] = FindClip("windup south");
            _windupAnimationClips[MathUtils.CardinalDirection.W] = FindClip("windup east");

            _advanceAnimationClips[MathUtils.CardinalDirection.W] = FindClip("advance east");
            _advanceAnimationClips[MathUtils.CardinalDirection.E] = FindClip("advance east");
            _advanceAnimationClips[MathUtils.CardinalDirection.S] = FindClip("advance south");
            _advanceAnimationClips[MathUtils.CardinalDirection.N] = FindClip("advance north");
        }

        public tk2dSpriteAnimationClip FindClip(string clipName) {
            var clip = _animator.GetClipByName(clipName);
            if (clip == null) {
                throw new ArgumentException("Required clip \"" + clipName + "\" not found.");
            }

            return clip;
        }

        public override IEnumerator SpecialAttack(ICombatant receiver, CombatantView receiverView, WeaponHitSeverity severity) {
            var windupClip = _windupAnimationClips[Facing];
            var advanceClip = _advanceAnimationClips[Facing];

            yield return StartCoroutine(PlayClip(windupClip));

            var advanceClipDuration = ((1000/advanceClip.fps)*advanceClip.frames.Length/1000);
            var destination = receiverView.transform.position;
            transform
                .DOMove(destination, advanceClipDuration)
                .SetEase(Ease.OutCubic)
                .Play();
            yield return StartCoroutine(PlayClip(advanceClip));
        }

        private IEnumerator PlayClip(tk2dSpriteAnimationClip clip) {
            bool complete = false;

            Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> onCompleteAction = null;
            onCompleteAction = (animator, playedClip) => {
                complete = true;
                _animator.AnimationCompleted -= onCompleteAction;
            };

            _animator.AnimationCompleted += onCompleteAction;
            _animator.Play(clip);

            while (!complete) {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}