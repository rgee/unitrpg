using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grid;

namespace Combat {
    public class LiatAnimator : UnitAnimator {
        private bool _woundUp;
        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _windupAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        void Start() {
            base.Start();
            _windupAnimationClips[MathUtils.CardinalDirection.E] = FindClip("windup east");
            _windupAnimationClips[MathUtils.CardinalDirection.N] = FindClip("windup north");
            _windupAnimationClips[MathUtils.CardinalDirection.S] = FindClip("windup south");
            _windupAnimationClips[MathUtils.CardinalDirection.W] = FindClip("windup west");
        }

        protected override void SetAttackAnimation() {
            if (_woundUp) {
                base.SetAttackAnimation();
                return;
            }

            _animator.Play(_windupAnimationClips[_unit.Facing]);
            _animator.AnimationCompleted = (animator, clip) => {
                _woundUp = true;
                _animator.AnimationCompleted = null;
                base.SetAttackAnimation();
            };
        }

        protected override void SetNotAttacking(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            _woundUp = false;
            base.SetNotAttacking(animator, clip);
        }
    }
}
