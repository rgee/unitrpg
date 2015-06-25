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

        private Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _CombatAnimationIds =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        void Start() {
            _unit = GetComponent<Unit>();
            _animator = GetComponent<tk2dSpriteAnimator>();

            _runningAnimationClips[MathUtils.CardinalDirection.E] = FindClip("run east");
            _runningAnimationClips[MathUtils.CardinalDirection.W] = FindClip("run west");
            _runningAnimationClips[MathUtils.CardinalDirection.N] = FindClip("run north");
            _runningAnimationClips[MathUtils.CardinalDirection.S] = FindClip("run south");

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
                SetCombatAnimation();
            } else {
                _animator.Play(_idleClip);
            }
        }

        void SetRunningAnimation() {
            _animator.Play(_runningAnimationClips[_unit.Facing]);
        }

        void SetCombatAnimation() {
            switch (_unit.Facing) {
                case MathUtils.CardinalDirection.E:
                    break;
                case MathUtils.CardinalDirection.N:
                    break;
                case MathUtils.CardinalDirection.S:
                    break;
                case MathUtils.CardinalDirection.W:
                    break;
            }
        }

    }
}
