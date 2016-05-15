using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Contexts.Battle.Utilities;
using Models.Fighting.Characters;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    [RequireComponent(typeof(FightPhaseAnimator))]
    [RequireComponent(typeof(CombatantController))]
    [RequireComponent(typeof(CombatantAnimator))]
    public class CombatantView : View {

        public string CharacterName;
        public string CombatantId;
        public ArmyType Army = ArmyType.Friendly;
        public CombatantState State = CombatantState.Idle;
        public MathUtils.CardinalDirection Facing = MathUtils.CardinalDirection.S;
        public float SecondsPerSquare = 0.3f;

        public Signal DeathSignal {
            get { return _animator.DeathSignal; }
        }

        public Signal AttackCompleteSignal {
            get { return _animator.AttackCompleteSignal; }
        }

        public Signal DodgeCompleteSignal {
            get { return _animator.DodgeCompleteSignal; }
        }

        public Signal AttackConnectedSignal {
            get { return _animator.AttackConnectedSignal; }
        }

        private FightPhaseAnimator _phaseAnimator;
        private CombatantController _controller;
        private CombatantAnimator _animator;

        void Awake() {
            base.Awake();

            _phaseAnimator = GetComponent<FightPhaseAnimator>();
            _controller = GetComponent<CombatantController>();
            _animator = GetComponent<CombatantAnimator>();
        }

        public IEnumerator DoAttack() {
            var attackComplete = false;
            var onComplete = new Action(() => {
                attackComplete = true;
            });

            _animator.AttackCompleteSignal.AddOnce(onComplete);
            State = CombatantState.Attacking;

            while (!attackComplete) {
                yield return new WaitForEndOfFrame();
            }
            State = CombatantState.CombatReady;
        }

        public IEnumerator FollowPath(IList<Vector3> path, MapDimensions dimensions) {
            return _controller.FollowPath(path, dimensions);
        }
    }
}