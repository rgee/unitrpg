using System;
using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Views;
using DG.Tweening;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Utilities {
    public class CombatantAnimator : MonoBehaviour {
        protected CombatantView View;
        protected tk2dSpriteAnimator Animator;

        public Signal DeathSignal = new Signal();
        public Signal AttackConnectedSignal = new Signal();
        public Signal AttackStartedSignal = new Signal();
        public Signal AttackCompleteSignal = new Signal();
        public Signal DodgeCompleteSignal = new Signal();

        private tk2dSpriteAnimationClip _idleClip;
        private tk2dSprite _sprite;

        private const string HitEventInfo = "hit";

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> RunningAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> CombatAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> AttackAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip> DodgeAnimationClips =
            new Dictionary<MathUtils.CardinalDirection, tk2dSpriteAnimationClip>();

        protected void Awake() {
            View = GetComponent<CombatantView>();
            Animator = GetComponent<tk2dSpriteAnimator>();
            _sprite = GetComponent<tk2dSprite>();

            RunningAnimationClips[MathUtils.CardinalDirection.E] = FindClip("run east");
            RunningAnimationClips[MathUtils.CardinalDirection.W] = FindClip("run east");
            RunningAnimationClips[MathUtils.CardinalDirection.N] = FindClip("run north");
            RunningAnimationClips[MathUtils.CardinalDirection.S] = FindClip("run south");

            CombatAnimationClips[MathUtils.CardinalDirection.N] = FindClip("combat north");
            CombatAnimationClips[MathUtils.CardinalDirection.S] = FindClip("combat south");
            CombatAnimationClips[MathUtils.CardinalDirection.E] = FindClip("combat east");
            CombatAnimationClips[MathUtils.CardinalDirection.W] = FindClip("combat east");

            AttackAnimationClips[MathUtils.CardinalDirection.N] = FindClip("attack north");
            AttackAnimationClips[MathUtils.CardinalDirection.S] = FindClip("attack south");
            AttackAnimationClips[MathUtils.CardinalDirection.E] = FindClip("attack east");
            AttackAnimationClips[MathUtils.CardinalDirection.W] = FindClip("attack east");

            DodgeAnimationClips[MathUtils.CardinalDirection.N] = FindClip("dodge north");
            DodgeAnimationClips[MathUtils.CardinalDirection.E] = FindClip("dodge east");
            DodgeAnimationClips[MathUtils.CardinalDirection.W] = FindClip("dodge east");
            DodgeAnimationClips[MathUtils.CardinalDirection.S] = FindClip("dodge south");

            _idleClip = FindClip("idle");

            Animator.AnimationEventTriggered = HandleHit;
        }

        void OnDestroy() {
            Animator.AnimationEventTriggered = null;
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
            if (clip.frames[frame].eventInfo == HitEventInfo) {
                AttackConnectedSignal.Dispatch();
            }
        }

        protected tk2dSpriteAnimationClip FindClip(string clipName) {
            var clip = Animator.GetClipByName(clipName);
            if (clip == null) {
                throw new ArgumentException("Required clip \"" + clipName + "\" not found.");
            }

            return clip;
        }

        void Update() {
            UpdateFacingFlip();
            switch (View.State) {
                case CombatantState.Idle:
                    Animator.Play(_idleClip);
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
            var inCorrectState = View.State != CombatantState.Idle;
            _sprite.FlipX = inCorrectState && View.Facing == MathUtils.CardinalDirection.W;
        }

        protected void SetDodgeAnimation() {
            Animator.Play(DodgeAnimationClips[View.Facing]);
            Animator.AnimationCompleted = SetNotDodging;
        }

        protected void SetRunningAnimation() {
            Animator.Play(RunningAnimationClips[View.Facing]);
        }

        protected virtual void SetAttackAnimation() {
            Animator.Play(AttackAnimationClips[View.Facing]);
            Animator.AnimationCompleted = SetNotAttacking;
            AttackStartedSignal.Dispatch();
        }

        protected void ResetCombatAnimation() {
            Animator.PlayFromFrame(CombatAnimationClips[View.Facing], 1);
        }

        protected void SetCombatAnimation() {
            Animator.Play(CombatAnimationClips[View.Facing]);
        }

        protected virtual void SetNotDodging(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            Animator.AnimationCompleted = null;
            ResetCombatAnimation();
            DodgeCompleteSignal.Dispatch();
        }

        protected virtual void SetNotAttacking(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            Animator.AnimationCompleted = null;
            ResetCombatAnimation();
            AttackCompleteSignal.Dispatch();
        }
    }
}