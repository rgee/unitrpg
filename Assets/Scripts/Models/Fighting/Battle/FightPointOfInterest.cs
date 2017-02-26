using Contexts.Battle.Utilities;
using UnityEngine;
using Utils;

namespace Models.Fighting.Battle {
    public class FightPointOfInterest : IPointOfInterest {
        public Vector3 FocalPoint { get; private set; }
        public float Tolerance { get; private set; }

        public FightPointOfInterest(MapDimensions dimensions, ICombatant attacker, ICombatant defender) {
            var attackerPosition = dimensions.GetWorldPositionForGridPosition(attacker.Position);
            var defenderPosition = dimensions.GetWorldPositionForGridPosition(defender.Position);
            FocalPoint = MathUtils.Midpoint(attackerPosition, defenderPosition);

            Tolerance = 1f;
        }
    }
}