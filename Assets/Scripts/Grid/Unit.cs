using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Grid {
    public class Unit : MonoBehaviour {

        public Vector2 gridPosition;
        public bool friendly;
		public Models.Unit model;
		public float timePerMoveSquare = 0.3f;
        private Seeker seeker;
        private ActionMenuManager menuManager;
        

        void Start() {
            menuManager = GameObject.FindGameObjectWithTag("ActionMenuManager").GetComponent<ActionMenuManager>();
            seeker = GetComponent<Seeker>();
            seeker.startEndModifier.exactEndPoint = Pathfinding.StartEndModifier.Exactness.SnapToNode;
        }

		public void TakeDamage(int damage) {
			model.TakeDamage(damage);
		}

		public delegate void OnPathingComplete(bool moved);

        public void Select() {
            menuManager.ShowActionMenu(this);
        }

        public void Deselect() {
            menuManager.HideCurrentMenu();
        }

		public IEnumerator MoveAlongPath(IList<Vector3> path, OnPathingComplete callback) {
			foreach (Vector3 point in path) {
				yield return StartCoroutine(MoveToPoint(point, timePerMoveSquare));
			}
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

        public void MoveTo(Vector2 pos, MapGrid grid, OnPathingComplete callback) {

            Vector3 destination = grid.GetWorldPosForGridPos(pos);
			seeker.StartPath(transform.position, destination, (p) => {
				if (!p.error) {
					StartCoroutine(MoveAlongPath(p.vectorPath, callback));
				} else {
					callback(false);
				}
			});
        }
    }
}

