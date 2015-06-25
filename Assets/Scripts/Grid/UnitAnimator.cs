using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Grid {
    [RequireComponent(typeof(tk2dSpriteAnimator), typeof(Unit))]
    public class UnitAnimator : MonoBehaviour {
        private Unit _unit;
        private tk2dSpriteAnimator _animator;

        private tk2dSpriteAnimationClip _idleClip;

        private Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _runningAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        private Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _combatAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();
        
        private Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _attackAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        void Start() {
            _unit = GetComponent<Unit>();
            _animator = GetComponent<tk2dSpriteAnimator>();

            _runningAnimationClips[MathUtils.CardinalDirection.E] = FindClip("run east");
            _runningAnimationClips[MathUtils.CardinalDirection.W] = FindClip("run west");
            _runningAnimationClips[MathUtils.CardinalDirection.N] = FindClip("run north");
            _runningAnimationClips[MathUtils.CardinalDirection.S] = FindClip("run south");

            _combatAnimationClips[MathUtils.CardinalDirection.N] = FindClip("combat north");
            _combatAnimationClips[MathUtils.CardinalDirection.S] = FindClip("combat south");

            _attackAnimationClips[MathUtils.CardinalDirection.N] = FindClip("attack north");
            _attackAnimationClips[MathUtils.CardinalDirection.S] = FindClip("attack south");

            _idleClip = FindClip("idle");
        }

        private tk2dSpriteAnimationClip FindClip(string name) {
            var clip = _animator.GetClipByName(name);
            if (clip == null) {
                throw new ArgumentException("Required clip \"" + name + "\" not found.");
            }

            return clip;
        }

        void Update() {
            if (_unit.Running) {
                SetRunningAnimation();
            } else if (_unit.InCombat) {
                if (_unit.Attacking) {
                    SetAttackAnimation(); 
                } else {
                    SetCombatAnimation();
                }
            } else {
                _animator.Play(_idleClip);
            }
        }

        void SetRunningAnimation() {
            _animator.Play(_runningAnimationClips[_unit.Facing]);
        }

        void SetAttackAnimation() {
            _animator.Play(_attackAnimationClips[_unit.Facing]);
            _animator.AnimationCompleted = SetNotAttacking;
        }

        void ResetCombatAnimation() {
            _animator.PlayFromFrame(_combatAnimationClips[_unit.Facing], 1);
        }

        void SetCombatAnimation() {
            _animator.Play(_combatAnimationClips[_unit.Facing]);
        }

        void SetNotAttacking(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            _unit.Attacking = false;
            _animator.AnimationCompleted = null;
            ResetCombatAnimation();
        }
    }
}
