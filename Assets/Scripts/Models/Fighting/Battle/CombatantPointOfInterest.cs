using Contexts.Battle.Utilities;
using UnityEngine;

namespace Models.Fighting.Battle {
    public class CombatantPointOfInterest : IPointOfInterest {
        public Vector3 FocalPoint { get; private set; }
        public float Tolerance { get; private set; }

        public CombatantPointOfInterest(MapDimensions dimensions, ICombatant combatant) {
            FocalPoint = dimensions.GetWorldPositionForGridPosition(combatant.Position);
            Tolerance = 1f;
        }
    }
}