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

        private Seeker seeker;
        private ActionMenuManager menuManager;
		private Animator animator;
		private Grid.Unit CurrentAttackTarget;
		private Hit CurrentHit;

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
        public event EventHandler OnDeath;
		public event EventHandler<AttackConnectedEventArgs> OnHitConnect;

        public Models.Character GetCharacter() {
            return model.Character;
        }

        void AttackComplete() {
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
			} else {
				SoundFX.Instance.PlayMiss();
			}

			// Only trigger this event if it wasn't a miss.
			if (OnHitConnect != null && !CurrentHit.Missed) {
				OnHitConnect(this, new AttackConnectedEventArgs(CurrentHit));
			}
		}

		void ShowCrit() {
			GameObject hitConfirmation = Instantiate(CritConfirmPrefab) as GameObject;
			hitConfirmation.transform.parent = CurrentAttackTarget.gameObject.transform;
			hitConfirmation.transform.localPosition = new Vector3();

			SoundFX.Instance.PlayCrit();
		}

		void ShowHit() {
			GameObject hitConfirmation = Instantiate(HitConfirmPrefab) as GameObject;
			hitConfirmation.transform.parent = CurrentAttackTarget.gameObject.transform;
			hitConfirmation.transform.localPosition = new Vector3();

			SoundFX.Instance.PlayHit();
		}

		void ShowGlance() {
			SoundFX.Instance.PlayGlance();
			GameObject hitConfirmation = Instantiate(GlanceConfirmPrefab) as GameObject;
			hitConfirmation.transform.parent = CurrentAttackTarget.gameObject.transform;
			hitConfirmation.transform.localPosition = new Vector3();

			SoundFX.Instance.PlayGlance();
		}

        void Prepared() {
            if (OnPreparedForCombat != null) {
                OnPreparedForCombat();
            }
        }

        void Dead() {
            if (OnDeath != null) {
                OnDeath(this, EventArgs.Empty);
            }
        }

        public void PrepareForCombat(MathUtils.CardinalDirection facing) {
            // TODO: Infer direction from target.
            animator.SetInteger("Direction", animatorDirections[facing]);
            animator.SetBool("In Combat", true);
        }

		public void Dodge() {
			animator.SetTrigger("Dodge");
		}

        public void Attack(Grid.Unit target, Hit hit) {
			this.CurrentAttackTarget = target;
			this.CurrentHit = hit;
            animator.SetTrigger("Attack");
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

        public void Select() {
            //menuManager.ShowActionMenu(this);
        }

        public void Deselect() {
            //menuManager.HideCurrentMenu();
        }

        public IEnumerator MoveAlongPath(List<Vector3> path) {
            yield return StartCoroutine(MoveAlongPath(path, (moved) => { }));
        }

		public IEnumerator MoveAlongPath(List<Vector3> path, OnPathingComplete callback) {
			animator.SetBool("Running", true);

			Vector3 prevPoint = transform.position;
			foreach (Vector3 point in path.GetRange(1, path.Count-1)) {
				MathUtils.CardinalDirection dir = MathUtils.DirectionTo(prevPoint, point);
				animator.SetInteger("Direction", animatorDirections[dir]);
				yield return StartCoroutine(MoveToPoint(point, timePerMoveSquare));
				prevPoint = point;
			}

			animator.SetBool("Running", false);
			callback(true);
		}

		public IEnumerator MoveToPoint(Vector3 dest, float time) {
			float elapsedTime = 0;
			Vector3 startPosition = transform.position;

			while (elapsedTime < time) {
				transform.position = Vector3.Lerp (startPosition, dest, (elapsedTime / time));
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}

        public void MoveTo(Vector2 pos, MapGrid grid, OnSearchComplete searchCb, OnPathingComplete callback) {

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
					StartCoroutine(MoveAlongPath(p.vectorPath, callback));
				} else {
					callback(false);
				}
			});
        }
    }
}

