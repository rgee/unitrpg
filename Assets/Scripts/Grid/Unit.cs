using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Grid {
    public class Unit : MonoBehaviour {

        public Vector2 gridPosition;
        public bool friendly;
		public Models.Unit model;
		public float timePerMoveSquare = 0.3f;
		public GameObject HitConfirmPrefab;
		public GameObject CritConfirmPrefab;
		public GameObject GlanceConfirmPrefab;

		public bool Attacking;
        public bool Killing;

        private Seeker seeker;
        private ActionMenuManager menuManager;
		private Animator animator;
		private Grid.Unit CurrentAttackTarget;
		private Hit CurrentHit;
		private UnitController Controller;

		public class AttackConnectedEventArgs : System.EventArgs {
			public AttackConnectedEventArgs(Hit hit) {
				this.hit = hit;
			}

			public readonly Hit hit;
		}

		private static Dictionary<MathUtils.CardinalDirection, int> animatorDirections = new Dictionary<MathUtils.CardinalDirection, int>() {
			{ MathUtils.CardinalDirection.W, 1},
			{ MathUtils.CardinalDirection.N, 2},
			{ MathUtils.CardinalDirection.E, 3},
			{ MathUtils.CardinalDirection.S, 0}
		};

        void Start() {
            menuManager = GameObject.FindGameObjectWithTag("ActionMenuManager").GetComponent<ActionMenuManager>();
            seeker = GetComponent<Seeker>();
            seeker.startEndModifier.exactEndPoint = Pathfinding.StartEndModifier.Exactness.SnapToNode;
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

        public delegate void OnSearchComplete(bool foundPath);
		public delegate void OnPathingComplete(bool moved);

        public delegate void CombatPreparationHandler();
        public delegate void AttackCompletionHandler();
        public delegate void DeathHandler();

        public event AttackCompletionHandler OnAttackComplete;
        public event CombatPreparationHandler OnPreparedForCombat;

        public Models.Character GetCharacter() {
            return model.Character;
        }

        void AttackComplete() {
			Attacking = false;
            Killing = false;

            if (OnAttackComplete != null) {
                OnAttackComplete();
            }
        }

		void AttackConnected() {
            if (CurrentHit.Crit) {
                ShowCrit();
            } else if (CurrentHit.Glanced) {
                ShowGlance();
            } else if (!CurrentHit.Missed) {
                ShowHit();
            }

            CombatEventBus.Hits.Dispatch(CurrentHit);
		}

		void ShowCrit() {
			GameObject hitConfirmation = Instantiate(CritConfirmPrefab) as GameObject;
			hitConfirmation.transform.parent = CurrentAttackTarget.gameObject.transform;
			hitConfirmation.transform.localPosition = new Vector3();

		}

		void ShowHit() {
			GameObject hitConfirmation = Instantiate(HitConfirmPrefab) as GameObject;
			hitConfirmation.transform.parent = CurrentAttackTarget.gameObject.transform;
			hitConfirmation.transform.localPosition = new Vector3();
		}

		void ShowGlance() {
			SoundFX.Instance.PlayGlance();
			GameObject hitConfirmation = Instantiate(GlanceConfirmPrefab) as GameObject;
			hitConfirmation.transform.parent = CurrentAttackTarget.gameObject.transform;
			hitConfirmation.transform.localPosition = new Vector3();
		}

        void Prepared() {
            if (OnPreparedForCombat != null) {
                OnPreparedForCombat();
            }
        }

        void Dead() {
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

        public void Attack(Grid.Unit target, Hit hit, bool killingBlow) {
			this.CurrentAttackTarget = target;
			this.CurrentHit = hit;
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
			bool complete = false;
			Controller.MoveAlongPath(path, () => complete = true);
			while (!complete) {
				yield return new WaitForEndOfFrame();
			}
        }

        public void MoveTo(Vector2 pos, MapGrid grid, OnSearchComplete searchCb, Action callback) {

            Vector3 destination = grid.GetWorldPosForGridPos(pos);

            // Remove this unit's collider so the pathfinder wont see the currently-occupied Grid square
            // as a blockage.
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
            grid.RescanGraph();
			seeker.StartPath(transform.position, destination, (p) => {

                // Re-enable the collider and scan the graph so other units see us.
                collider.enabled = true;
                grid.RescanGraph();
                searchCb(!p.error);
				if (!p.error) {
					List<Vector3> trimmedPath = p.vectorPath.GetRange(1, p.vectorPath.Count-1);
					Controller.MoveAlongPath(trimmedPath, callback);
				} else {
					callback();
				}
			});
        }
    }
}

