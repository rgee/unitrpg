using System;
using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Views;
using DG.Tweening;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Utilities {
    public class CombatantAnimator : MonoBehaviour {
        protected CombatantView _view;
        protected tk2dSpriteAnimator _animator;

        public Signal DeathSignal = new Signal();
        public Signal AttackConnectedSignal = new Signal();
        public Signal AttackStartedSignal = new Signal();
        public Signal AttackCompleteSignal = new Signal();
        public Signal DodgeCompleteSignal = new Signal();

        private tk2dSpriteAnimationClip _idleClip;
        private tk2dSprite _sprite;

        private static readonly string HIT_EVENT_INFO = "hit";

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _runningAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _combatAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _attackAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> _dodgeAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected void Awake() {
            _view = GetComponent<CombatantView>();
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

            _animator.AnimationEventTriggered = HandleHit;
        }

        void OnDestroy() {
            _animator.AnimationEventTriggered = null;
        }

        public IEnumerator FadeIn() {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0);
            yield return _sprite.DOFade(1f, 0.7f).WaitForCompletion();
        }

        public IEnumerator FadeToDeath(float time) {
            yield return _sprite.DOFade(0f, time).WaitForCompletion();
            DeathSignal.Dispatch();
        }

        public IEnumerator SlideTo(Vector3 destination) {
            yield return transform.DOMove(destination, 0.3f).SetEase(Ease.OutCubic).WaitForCompletion();
        }

        private void HandleHit(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frame) {
            if (clip.frames[frame].eventInfo == HIT_EVENT_INFO) {
                AttackConnectedSignal.Dispatch();
            }
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
            switch (_view.State) {
                case CombatantState.Idle:
                    _animator.Play(_idleClip);
                    break;
                case CombatantState.Attacking:
                    SetAttackAnimation();
                    break;
                case CombatantState.Dodging:
                    SetDodgeAnimation();
                    break;
                case CombatantState.CombatReady:
                    SetCombatAnimation();
                    break;
                case CombatantState.Running:
                    SetRunningAnimation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void UpdateFacingFlip() {
            var inCorrectState = _view.State == CombatantState.CombatReady ||
                                 _view.State == CombatantState.Running;
            _sprite.FlipX = inCorrectState && _view.Facing == MathUtils.CardinalDirection.W;
        }

        protected void SetDodgeAnimation() {
            _animator.Play(_dodgeAnimationClips[_view.Facing]);
            _animator.AnimationCompleted = SetNotDodging;
        }

        protected void SetRunningAnimation() {
            _animator.Play(_runningAnimationClips[_view.Facing]);
        }

        protected virtual void SetAttackAnimation() {
            _animator.Play(_attackAnimationClips[_view.Facing]);
            _animator.AnimationCompleted = SetNotAttacking;
            AttackStartedSignal.Dispatch();
        }

        protected void ResetCombatAnimation() {
            _animator.PlayFromFrame(_combatAnimationClips[_view.Facing], 1);
        }

        protected void SetCombatAnimation() {
            _animator.Play(_combatAnimationClips[_view.Facing]);
        }

        protected virtual void SetNotDodging(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            _animator.AnimationCompleted = null;
            ResetCombatAnimation();
            DodgeCompleteSignal.Dispatch();
        }

        protected virtual void SetNotAttacking(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            _animator.AnimationCompleted = null;
            ResetCombatAnimation();
            AttackCompleteSignal.Dispatch();
        }
    }
}