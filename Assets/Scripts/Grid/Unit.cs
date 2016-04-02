using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Assets.Models.Combat;
using Combat;
using DG.Tweening;
using Models;
using Pathfinding;
using UnityEngine;

namespace Grid {
    public class Unit : MonoBehaviour {
        public delegate void AttackCompletionHandler();

        public delegate void CombatPreparationHandler();

        public delegate void DeathHandler();

        public delegate void OnPathingComplete(bool moved);

        public delegate void OnSearchComplete(bool foundPath);

        private static readonly Dictionary<MathUtils.CardinalDirection, int> animatorDirections =
            new Dictionary<MathUtils.CardinalDirection, int> {
                {MathUtils.CardinalDirection.W, 1},
                {MathUtils.CardinalDirection.N, 2},
                {MathUtils.CardinalDirection.E, 3},
                {MathUtils.CardinalDirection.S, 0}
            };

        public bool friendly;
        public Vector2 gridPosition;
        public bool Killing;
        public Models.Combat.Unit model;
        public bool IsDodging;
        public Unit CurrentAttackTarget;
        public float timePerMoveSquare = 0.3f;

        public event AttackCompletionHandler OnAttackComplete;
        public event Action OnAttackStart;
        public event Action OnDodgeComplete;

        private UnitController Controller;
        private Hit CurrentHit;
        private Seeker seeker;
        private Collider _collider;

        public bool Attacking;
        public bool InCombat;
        public MathUtils.CardinalDirection Facing = MathUtils.CardinalDirection.S;
        public bool Running;

        void Start() {
            seeker = GetComponent<Seeker>();
            seeker.startEndModifier.exactEndPoint = StartEndModifier.Exactness.SnapToNode;
            Controller = GetComponent<UnitController>();
            model.Health = model.Character.MaxHealth;
            _collider = GetComponent<Collider>();
        }

        public void SnapToGrid() {
        }

        public bool IsAlive() {
            return model.IsAlive;
        }

        public void TakeDamage(int damage) {
            model.TakeDamage(damage);
            if (!model.IsAlive) {
                //animator.SetTrigger("Dead");
            }
        }

        public Character GetCharacter() {
            return model.Character;
        }

        public void AttackComplete() {
            Attacking = false;
            Killing = false;

            if (OnAttackComplete != null) {
                OnAttackComplete();
            }
        }

        public void DisableCollision() {
            _collider.enabled = false;
        }

        public void EnableCollision() {
            _collider.enabled = true;
        }

        public void AttackBegin() {
            if (OnAttackStart != null) {
                OnAttackStart();
            }
        }

        public void DodgeComplete() {
            IsDodging = false;
            if (OnDodgeComplete != null) {
                OnDodgeComplete();
            }
        }

        public void AttackConnected() {
            // This can happen if you're just tweaking around with animations in the editor.
            if (CurrentAttackTarget == null) {
                Debug.LogWarning("Unit " + model.Character.Name + " attacked nothing.");
                return;
            }

            CombatEventBus.HitEvents.Dispatch(new HitEvent {
                Target = CurrentAttackTarget.gameObject,
                Data = CurrentHit,
                Attacker = gameObject
            });
        }

        private void Dead() {
            CombatEventBus.Deaths.Dispatch(this);
        }

        public void PrepareForCombat(MathUtils.CardinalDirection facing) {
            Facing = facing;
            InCombat = true;
        }

        public void Dodge() {
            IsDodging = true;
        }

        public void Attack(Unit target, Hit hit, bool killingBlow) {
            CurrentAttackTarget = target;
            CurrentHit = hit;
            Attacking = true;
            Killing = killingBlow;
        }

        public void ReturnToRest() {
            InCombat = false;
        }

        public bool CanLevel() {
            return model.Character.Exp >= 100;
        }

        public void ApplyExp(int amt) {
            model.Character.ApplyExp(amt);
        }

        public IEnumerator FollowPath(List<Vector3> path) {
            yield return StartCoroutine(Controller.FollowPath(path));
        }

        public IEnumerator MoveTo(Vector2 pos, MapGrid grid, IMovementEventHandler movementEventHandler) {
            var worldSpaceDestination = grid.GetWorldPosForGridPos(pos);
            DisableCollision();
            grid.RescanGraph();

            var seekingComplete = false;
            List<Vector3> path = null;
            seeker.StartPath(transform.position, worldSpaceDestination, result => {
                seekingComplete = true;
                if (!result.error) {
                    path = result.vectorPath.GetRange(1, result.vectorPath.Count - 1);
                }
            });

            // Block on the Seeker becuase it doesn't expose a Coroutine-based API.
            while (!seekingComplete) {
                yield return null;
            }

            if (path != null) {
                yield return StartCoroutine(Controller.FollowPath(path, movementEventHandler));
            }
        }
    }
}