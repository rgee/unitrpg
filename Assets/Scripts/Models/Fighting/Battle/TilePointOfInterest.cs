using Contexts.Battle.Utilities;
using UnityEngine;

namespace Models.Fighting.Battle {
    public class TilePointOfInterest : IPointOfInterest {
        public Vector3 FocalPoint { get; private set; }
        public float Tolerance { get; private set; }

        public TilePointOfInterest(MapDimensions dimensions, Vector2 location) {
            FocalPoint = dimensions.GetWorldPositionForGridPosition(location);
            Tolerance = 1f;
        }
    }
}