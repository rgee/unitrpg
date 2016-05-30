using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Contexts;
using Contexts.Battle.Utilities;
using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Effects;
using Models.Fighting.Execution;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;
using Utils;

namespace Contexts.Battle.Views {
    [RequireComponent(typeof(CombatantController))]
    [RequireComponent(typeof(CombatantAnimator))]
    public class CombatantView : View {

        public string CharacterName;
        public string CombatantId;
        public ArmyType Army = ArmyType.Friendly;
        public CombatantState State = CombatantState.Idle;
        public MathUtils.CardinalDirection Facing = MathUtils.CardinalDirection.S;
        public float SecondsPerSquare = 0.3f;

        public Signal DeathSignal = new Signal();

        public Signal AttackCompleteSignal = new Signal();

        public Signal DodgeCompleteSignal = new Signal();

        public Signal<WeaponHitConnection> AttackConnectedSignal = new Signal<WeaponHitConnection>();

        private CombatantController _controller;
        private tk2dSprite _sprite;
        private CombatantAnimator _animator;

        protected virtual void Awake() {
            base.Awake();

            _sprite = GetComponent<tk2dSprite>();
            _controller = GetComponent<CombatantController>();
            _animator = GetComponent<CombatantAnimator>();

            StrangeUtils.Bubble(_animator.DeathSignal, DeathSignal);
            StrangeUtils.Bubble(_animator.AttackCompleteSignal, AttackCompleteSignal);
            StrangeUtils.Bubble(_animator.DodgeCompleteSignal, DodgeCompleteSignal);
        }

        public void Update() {
            UpdateFacingFlip();
        }

        public void PrepareForCombat(MathUtils.CardinalDirection direction) {
            Facing = direction;
            State = CombatantState.CombatReady;
        }

        public void ReturnToRest() {
            State = CombatantState.Idle;
        }

        protected void UpdateFacingFlip() {
            var inCorrectState = State != CombatantState.Idle;
            _sprite.FlipX = inCorrectState && Facing == MathUtils.CardinalDirection.W;
        }

        public IEnumerator Attack(ICombatant receiver, WeaponHitSeverity severity) {
            var attackComplete = false;
            var onComplete = new Action(() => {
                attackComplete = true;
            });

            _animator.AttackConnectedSignal.AddOnce(() => {
                var connection = new WeaponHitConnection(severity, receiver);
                AttackConnectedSignal.Dispatch(connection);
            });

            _animator.AttackCompleteSignal.AddOnce(onComplete);
            State = CombatantState.Attacking;

            while (!attackComplete) {
                yield return new WaitForEndOfFrame();
            }
            State = CombatantState.CombatReady;
        }

        public virtual IEnumerator SpecialAttack(FightPhase phase, MapDimensions dimensions, CombatantView receiverView, WeaponHitSeverity severity) {
            throw new InvalidOperationException("Combatant: " + CombatantId + " has no special attack.");
        }

        public IEnumerator Dodge() {
            var dodgeComplete = false;
            _animator.DodgeCompleteSignal.AddOnce(() => {
                dodgeComplete = true;
            });
            State = CombatantState.Dodging;

            while (!dodgeComplete) {
                yield return new WaitForEndOfFrame();
            }

            State = CombatantState.CombatReady;
        }

        public IEnumerator FollowPath(IList<Vector3> path, MapDimensions dimensions) {
            return _controller.FollowPath(path, dimensions);
        }

        public void Die() {
            StartCoroutine(_animator.FadeToDeath(0.7f));
        }
    }
}