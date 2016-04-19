using Grid;
using Models.Fighting.Maps;
using UnityEngine;

namespace Assets.Testing {
    public class MapProbingTestScene : MonoBehaviour {
        void Start() {
            var mapView = MapManager.Instance;
            var obstclePositions = mapView.GetObstacles();

            var map = new Map(mapView.Width, mapView.Height);
            foreach (var obstacle in obstclePositions) {
                Debug.Log("Obstructing " + obstacle);
                map.AddObstruction(obstacle);
            }
        }
    }
}