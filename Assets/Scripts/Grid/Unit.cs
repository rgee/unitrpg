using UnityEngine;
using System.Collections;

namespace Grid {
    public class Unit : MonoBehaviour {

        public Vector2 gridPosition;
        private Seeker seeker;

        void Start() {
            seeker = GetComponent<Seeker>();
        }

        public void MoveTo(Vector2 pos, MapGrid grid) {
            Debug.Log("Start: " + transform.position);
            Vector3 destination = grid.GetWorldPosForGridPos(pos);
            Debug.Log("Dest: " + destination);
            seeker.StartPath(transform.position, destination, OnPathFound);
        }

        private void OnPathFound(Pathfinding.Path p) {
            if (p.vectorPath.Count > 0) {
                Vector3 end = p.vectorPath[p.vectorPath.Count - 1];
                transform.position = end;
            }
        }
    }
}

