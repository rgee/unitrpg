using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Grid {
    [RequireComponent(typeof(tk2dSpriteAnimator), typeof(tk2dSprite), typeof(Unit))]
    public class UnitAnimator : MonoBehaviour {
        protected Unit _unit;
        protected tk2dSpriteAnimator _animator;

        private tk2dSpriteAnimationClip _idleClip;
        private tk2dSprite _sprite;

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _runningAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _combatAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();
        
        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _attackAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _dodgeAnimationClips = 
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected void Start() {
            _unit = GetComponent<Unit>();
            _animator = GetComponent<tk2dSpriteAnimator>();
            _sprite = GetComponent<tk2dSprite>();

            _runningAnimationClips[MathUtils.CardinalDirection.E] = FindClip("run east");
            _runningAnimationClips[MathUtils.CardinalDirection.W] = FindClip("run east");
            _runningAnimationClips[MathUtils.CardinalDirection.N] = FindClip("run north");
            _runningAnimationClips[MathUtils.CardinalDirection.S] = FindClip("run south");

            _combatAnimationClips[MathUtils.CardinalDirection.N] = FindClip("combat north");
            _combatAnimationClips[MathUtils.CardinalDirection.S] = FindClip("combat south");
            _combatAnimationClips[MathUtils.CardinalDirection.E] = FindClip("combat east");
            _combatAnimationClips[MathUtils.CardinalDirection.W] = FindClip("combat east");

            _attackAnimationClips[MathUtils.CardinalDirection.N] = FindClip("attack north");
            _attackAnimationClips[MathUtils.CardinalDirection.S] = FindClip("attack south");
            _attackAnimationClips[MathUtils.CardinalDirection.E] = FindClip("attack east");
            _attackAnimationClips[MathUtils.CardinalDirection.W] = FindClip("attack east");

            _dodgeAnimationClips[MathUtils.CardinalDirection.N] = FindClip("dodge north");
            _dodgeAnimationClips[MathUtils.CardinalDirection.E] = FindClip("dodge east");
            _dodgeAnimationClips[MathUtils.CardinalDirection.W] = FindClip("dodge east");
            _dodgeAnimationClips[MathUtils.CardinalDirection.S] = FindClip("dodge south");

            _idleClip = FindClip("idle");
        }

        protected tk2dSpriteAnimationClip FindClip(string name) {
            var clip = _animator.GetClipByName(name);
            if (clip == null) {
                throw new ArgumentException("Required clip \"" + name + "\" not found.");
            }

            return clip;
        }

        void Update() {
            UpdateFacingFlip();
            if (_unit.Running) {
                SetRunningAnimation();
            } else if (_unit.InCombat) {
                if (_unit.Attacking) {
                    SetAttackAnimation();
                } else if (_unit.IsDodging) {
                    SetDodgeAnimation();
                } else {
                    SetCombatAnimation();
                }
            } else {
                _animator.Play(_idleClip);
            }
        }

        protected void UpdateFacingFlip() {
            _sprite.FlipX = (_unit.InCombat || _unit.Running) && _unit.Facing == MathUtils.CardinalDirection.W;
        }

        protected void SetDodgeAnimation() {
            _animator.Play(_dodgeAnimationClips[_unit.Facing]);
            _animator.AnimationCompleted = SetNotDodging;
        }

        protected void SetRunningAnimation() {
            _animator.Play(_runningAnimationClips[_unit.Facing]);
        }

        protected virtual void SetAttackAnimation() {
            _animator.Play(_attackAnimationClips[_unit.Facing]);
            _animator.AnimationCompleted = SetNotAttacking;
        }

        protected void ResetCombatAnimation() {
            _animator.PlayFromFrame(_combatAnimationClips[_unit.Facing], 1);
        }

        protected void SetCombatAnimation() {
            _animator.Play(_combatAnimationClips[_unit.Facing]);
        }

        protected virtual void SetNotDodging(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            _unit.IsDodging = false;
            _animator.AnimationCompleted = null;
            ResetCombatAnimation();
        }

        protected virtual void SetNotAttacking(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            _unit.Attacking = false;
            _animator.AnimationCompleted = null;
            ResetCombatAnimation();
        }
    }
}
