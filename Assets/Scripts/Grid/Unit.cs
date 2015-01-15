using UnityEngine;
using System.Collections;

namespace Grid {
    public class Unit : MonoBehaviour {

        public Vector2 gridPosition;
		public Models.Unit model;
        private Seeker seeker;

        void Start() {
            seeker = GetComponent<Seeker>();
            seeker.startEndModifier.exactEndPoint = Pathfinding.StartEndModifier.Exactness.SnapToNode;
        }

		public void TakeDamage(int damage) {
			model.TakeDamage(damage);
		}

		public delegate void OnPathingComplete(bool moved);

        public void MoveTo(Vector2 pos, MapGrid grid, OnPathingComplete callback) {

            Vector3 destination = grid.GetWorldPosForGridPos(pos);
            Debug.Log("Finding Path from " + transform.position + " to " + destination);
			seeker.StartPath(transform.position, destination, (p) => {
				if (!p.error) {
					Vector3 end = p.vectorPath[p.vectorPath.Count - 1];
					transform.position = end;
					callback(true);
				} else {
					callback(false);
				}
			});
        }
    }
}

