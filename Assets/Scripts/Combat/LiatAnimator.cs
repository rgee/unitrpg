using System.Collections.Generic;
using DG.Tweening;
using Grid;
using UnityEngine;

namespace Combat {
    public class LiatAnimator : UnitAnimator {
        /// <summary>
        /// Whether or not the wind up animation has completed.
        /// </summary>
        private bool _woundUp;

        /// <summary>
        /// Whether or not Liat is going to advance on her attack.
        /// </summary>
        private bool _advancing;
        
        /// <summary>
        /// The destination to which Liat will advance.
        /// </summary>
        private Vector3 _advancingDestination;

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

        public void Advance(Vector3 destination) {
            _advancingDestination = destination;
            _advancing = true;
            GetComponent<Grid.Unit>().Attacking = true;
        }

        protected override void SetAttackAnimation() {
            if (_woundUp) {
                if (_advancing) {
                    var clip = _advanceAnimationClips[_unit.Facing];
                    var duration = ((1000/clip.fps)*clip.frames.Length/1000);
                    transform.DOMove(_advancingDestination, duration).SetEase(Ease.OutCubic).Play();
                    _animator.Play(clip);
                    _animator.AnimationCompleted = (animator, playedClip) => {
                        _advancing = false;
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
