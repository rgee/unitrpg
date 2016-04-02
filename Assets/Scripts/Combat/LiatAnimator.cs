using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Grid;
using UnityEngine;

namespace Combat {
    public class LiatAnimator : UnitAnimator {
        private bool _woundUp;
        public bool Advancing;
        public Vector3 AdvancingDestination;
        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _windupAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        private readonly Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _advanceAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        new void Awake() {
            base.Awake();
            _windupAnimationClips[MathUtils.CardinalDirection.E] = FindClip("windup east");
            _windupAnimationClips[MathUtils.CardinalDirection.N] = FindClip("windup north");
            _windupAnimationClips[MathUtils.CardinalDirection.S] = FindClip("windup south");
            _windupAnimationClips[MathUtils.CardinalDirection.W] = FindClip("windup east");

            _advanceAnimationClips[MathUtils.CardinalDirection.W] = FindClip("advance east");
            _advanceAnimationClips[MathUtils.CardinalDirection.E] = FindClip("advance east");
        }

        protected override void SetAttackAnimation() {
            if (_woundUp) {
                if (Advancing) {
                    var clip = _advanceAnimationClips[_unit.Facing];
                    var duration = ((1000/clip.fps)*clip.frames.Length/1000);
                    transform.DOMove(AdvancingDestination, duration).SetEase(Ease.OutCubic).Play();
                    _animator.Play(clip);
                    _animator.AnimationCompleted = (animator, playedClip) => {
                        Advancing = false;
                        SetNotAttacking(animator, playedClip);
                    };
                } else {
                    base.SetAttackAnimation();
                }
                return;
            }

            _animator.Play(_windupAnimationClips[_unit.Facing]);
            _animator.AnimationCompleted = (animator, clip) => {
                _woundUp = true;
                _animator.AnimationCompleted = null;
                if (_unit.model.Character.AttackRange > 1) {
                    // TODO: Also check if the target is sufficiently far
                } else {
                    base.SetAttackAnimation();
                }
            };
        }

        private void SetBowAttackAnimation() {
            
        }

        protected override void SetNotAttacking(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            _woundUp = false;
            base.SetNotAttacking(animator, clip);
        }
    }
}
