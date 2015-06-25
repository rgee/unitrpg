using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
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

        public bool Attacking;
        public bool friendly;
        public Vector2 gridPosition;
        public bool Killing;
        public Models.Combat.Unit model;
        public bool IsDodging = false;
        public float timePerMoveSquare = 0.3f;

        public event AttackCompletionHandler OnAttackComplete;
        public event CombatPreparationHandler OnPreparedForCombat;
        public event Action OnAttackStart;
        public event Action OnDodgeComplete;

        private Animator animator;
        private UnitController Controller;
        private Unit CurrentAttackTarget;
        private Hit CurrentHit;
        private Seeker seeker;

        public bool InCombat;
        public MathUtils.CardinalDirection Facing;
        public bool Running;

        void Start() {
            seeker = GetComponent<Seeker>();
            seeker.startEndModifier.exactEndPoint = StartEndModifier.Exactness.SnapToNode;
            animator = GetComponent<Animator>();
            Controller = GetComponent<UnitController>();
            model.Health = model.Character.MaxHealth;
        }

        public bool IsAlive() {
            return model.IsAlive;
        }

        public void TakeDamage(int damage) {
            model.TakeDamage(damage);
            if (!model.IsAlive) {
                animator.SetTrigger("Dead");
            }
        }

        public Character GetCharacter() {
            return model.Character;
        }

        private void AttackComplete() {
            Attacking = false;
            Killing = false;

            if (OnAttackComplete != null) {
                OnAttackComplete();
            }
        }

        private void AttackBegin() {
            if (OnAttackStart != null) {
                OnAttackStart();
            }
        }

        private void DodgeComplete() {
            if (OnDodgeComplete != null) {
                OnDodgeComplete();
            }
        }

        private void AttackConnected() {
            CombatEventBus.HitEvents.Dispatch(new HitEvent {
                Target = CurrentAttackTarget.gameObject,
                Data = CurrentHit,
                Attacker = gameObject
            });
        }

        private void Prepared() {
            if (OnPreparedForCombat != null) {
                OnPreparedForCombat();
            }
        }

        private void Dead() {
            CombatEventBus.Deaths.Dispatch(this);
        }

        public void PrepareForCombat(MathUtils.CardinalDirection facing) {
            // TODO: Infer direction from target.
            animator.SetInteger("Direction", animatorDirections[facing]);
            animator.SetBool("In Combat", true);
        }

        public void Dodge() {
            animator.SetTrigger("Dodge");
        }

        public void Attack(Unit target, Hit hit, bool killingBlow) {
            CurrentAttackTarget = target;
            CurrentHit = hit;
            animator.SetTrigger("Attack");
            Attacking = true;
            Killing = killingBlow;
        }

        public void ReturnToRest() {
            animator.SetBool("In Combat", false);
        }

        public bool CanLevel() {
            return model.Character.Exp >= 100;
        }

        public void ApplyExp(int amt) {
            model.Character.ApplyExp(amt);
        }

        public IEnumerator MoveAlongPath(List<Vector3> path) {
            var complete = false;
            Controller.MoveAlongPath(path, () => complete = true);
            while (!complete) {
                yield return new WaitForEndOfFrame();
            }
        }

        public void MoveTo(Vector2 pos, MapGrid grid, Action onMovementComplete) {
            MoveTo(pos, grid, arg => { }, onMovementComplete);
        }

        public void MoveTo(Vector2 pos, MapGrid grid, OnSearchComplete searchCb, Action callback) {
            var destination = grid.GetWorldPosForGridPos(pos);

            // Remove this unit's collider so the pathfinder wont see the currently-occupied Grid square
            // as a blockage.
            var collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
            grid.RescanGraph();
            seeker.StartPath(transform.position, destination, p => {
                // Re-enable the collider and scan the graph so other units see us.
                collider.enabled = true;
                grid.RescanGraph();
                searchCb(!p.error);
                if (!p.error) {
                    var trimmedPath = p.vectorPath.GetRange(1, p.vectorPath.Count - 1);
                    Controller.MoveAlongPath(trimmedPath, callback);
                } else {
                    callback();
                }
            }, 1 << 0);
        }
    }
}