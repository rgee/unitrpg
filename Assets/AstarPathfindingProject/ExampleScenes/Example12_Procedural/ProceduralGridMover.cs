using UnityEngine;
using System.Collections;
using Pathfinding;

public class ProceduralGridMover : MonoBehaviour {
	
	public float updateDistance = 5;

	public Transform target;

	/** Flood fill the graph after updating.
	 * If this is set to false, areas of the graph will not be recalculated.
	 * Enable this only if the graph will only have a single area (i.e
	 * from all walkable nodes there is a valid path to every other walkable
	 * node). One case where this might be appropriate is a large
	 * outdoor area such as a forrest.
	 * If there are multiple areas in the graph and this
	 * is not enabled, pathfinding could fail later on.
	 * 
	 * Enabling it will make the graph updates faster.
	 */
	public bool floodFill;
	
	/** Grid graph to update */
	GridGraph graph;

	GridNode[] tmp;

	public void Start () {
		if ( AstarPath.active == null ) throw new System.Exception ("There is no AstarPath object in the scene");

		graph = AstarPath.active.astarData.gridGraph;

		if ( graph == null ) throw new System.Exception ("The AstarPath object has no GridGraph");
		UpdateGraph ();
	}

	// Update is called once per frame
	void Update () {

		// Check the distance along the XZ plane
		// we only move the graph in the X and Z directions so
		// checking for if the Y coordinate has changed is
		// not something we want to do
		if ( AstarMath.SqrMagnitudeXZ (target.position, graph.center) > updateDistance*updateDistance ) {
			UpdateGraph ();
		}
	}

	public void UpdateGraph () {
		// Start a work item for updating the graph
		// This will pause the pathfinding threads
		// so that it is safe to update the graph
		// and then do it over several frames
		// (hence the IEnumerator coroutine)
		// to avoid too large FPS drops
		IEnumerator ie = UpdateGraphCoroutine ();
		AstarPath.active.AddWorkItem (new AstarPath.AstarWorkItem (delegate (bool force) {
			if ( force ) while ( ie.MoveNext () ) {}
			return !ie.MoveNext ();
		}));
	}

	IEnumerator UpdateGraphCoroutine () {

		// Find the direction
		// that we want to move the graph in
		Vector3 dir = target.position - graph.center;

		// Snap to a whole number of nodes
		dir.x = Mathf.Round(dir.x/graph.nodeSize)*graph.nodeSize;
		dir.z = Mathf.Round(dir.z/graph.nodeSize)*graph.nodeSize;
		dir.y = 0;

		// Nothing do to
		if ( dir == Vector3.zero ) yield break;

		// Number of nodes to offset in each
		// direction
		Int2 offset = new Int2 ( -Mathf.RoundToInt(dir.x/graph.nodeSize), -Mathf.RoundToInt(dir.z/graph.nodeSize) );

		// Move the center
		graph.center += dir;
		graph.GenerateMatrix ();

		// Create a temporary buffer
		// required for the calculations
		if ( tmp == null || tmp.Length != graph.nodes.Length ) {
			tmp = new GridNode[graph.nodes.Length];
		}

		// Cache some variables for easier access
		int width = graph.width;
		int depth = graph.depth;
		GridNode[] nodes = graph.nodes;

		// Check if we have moved
		// less than a whole graph
		// width in any direction
		if ( Mathf.Abs(offset.x) <= width && Mathf.Abs(offset.y) <= depth ) {
		
			// Offset each node by the #offset variable
			// nodes which would end up outside the graph
			// will wrap around to the other side of it
			for ( int z=0; z < depth; z++ ) {
				int pz = z*width;
				int tz = ((z+offset.y + depth)%depth)*width;
				for ( int x=0; x < width; x++ ) {
					tmp[tz + ((x+offset.x + width) % width)] = nodes[pz + x];
				}
			}
			
			yield return null;

			// Copy the nodes back to the graph
			// and set the correct indices
			for ( int z=0; z < depth; z++ ) {
				int pz = z*width;
				for ( int x=0; x < width; x++ ) {
					GridNode node = tmp[pz + x];
					node.NodeInGridIndex = pz + x;
					nodes[pz + x] = node;
				}
			}


			IntRect r = new IntRect ( 0, 0, offset.x, offset.y );
			int minz = r.ymax;
			int maxz = depth;

			// If offset.x < 0, adjust the rect
			if ( r.xmin > r.xmax ) {
				int tmp2 = r.xmax;
				r.xmax = width + r.xmin;
				r.xmin = width + tmp2;
			}

			// If offset.y < 0, adjust the rect
			if ( r.ymin > r.ymax ) {
				int tmp2 = r.ymax;
				r.ymax = depth + r.ymin;
				r.ymin = depth + tmp2;
	
				minz = 0;
				maxz = r.ymin;
			}

			// Make sure erosion is taken into account
			// Otherwise we would end up with ugly artifacts
			r = r.Expand ( graph.erodeIterations + 1 );

			// Makes sure the rect stays inside the grid
			r = IntRect.Intersection ( r, new IntRect ( 0, 0, width, depth ) );
	
			yield return null;

			// Update all nodes along one edge of the graph
			// With the same width as the rect
			for ( int z = r.ymin; z < r.ymax; z++ ) {
				for ( int x = 0; x < width; x++ ) {
					graph.UpdateNodePositionCollision ( nodes[z*width + x], x, z, false );
				}
			}
	
			yield return null;
		
			// Update all nodes along the other edge of the graph
			// With the same width as the rect
			for ( int z = minz; z < maxz; z++ ) {
				for ( int x = r.xmin; x < r.xmax; x++ ) {
					graph.UpdateNodePositionCollision ( nodes[z*width + x], x, z, false );
				}
			}
	
			yield return null;

			// Calculate all connections for the nodes
			// that might have changed
			for ( int z = r.ymin; z < r.ymax; z++ ) {
				for ( int x = 0; x < width; x++ ) {
					graph.CalculateConnections (nodes, x, z, nodes[z*width+x]);
				}
			}
	
			yield return null;
	
			// Calculate all connections for the nodes
			// that might have changed
			for ( int z = minz; z < maxz; z++ ) {
				for ( int x = r.xmin; x < r.xmax; x++ ) {
					graph.CalculateConnections (nodes, x, z, nodes[z*width+x]);
				}
			}
	
			yield return null;

			// Calculate all connections for the nodes along the boundary
			// of the graph, these always need to be updated
			/** \todo Optimize to not traverse all nodes in the graph, only those at the edges */
			for ( int z = 0; z < depth; z++ ) {
				for ( int x = 0; x < width; x++ ) {
					if ( x == 0 || z == 0 || x >= width-1 || z >= depth-1 ) graph.CalculateConnections (nodes, x, z, nodes[z*width+x]);
				}
			}
			
		} else {

			// Just update all nodes
			for ( int z = 0; z < depth; z++ ) {
				for ( int x = 0; x < width; x++ ) {
					graph.UpdateNodePositionCollision ( nodes[z*width + x], x, z, false );
				}
			}

			// Recalculate the connections of all nodes
			for ( int z = 0; z < depth; z++ ) {
				for ( int x = 0; x < width; x++ ) {
					graph.CalculateConnections (nodes, x, z, nodes[z*width+x]);
				}
			}
		}
		
		if ( floodFill ) {
			yield return null;
			// Make sure the areas for the graph
			// have been recalculated
			// not doing this can cause pathfinding to fail
			AstarPath.active.QueueWorkItemFloodFill ();
		}
	}
}
