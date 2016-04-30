using System.Collections.Generic;
using Models.Fighting;
using UnityEngine;

namespace Contexts.Battle.Utilities {
    public class MovementPath {
        public readonly List<Vector2> Positions;
        public ICombatant Combatant;

        public MovementPath(List<Vector2> positions, ICombatant combatant) {
            Positions = positions;
            Combatant = combatant;
        }
    }
}