using System.Collections.Generic;
using Models.Fighting;
using Models.Fighting.Battle;
using UnityEngine;

namespace Contexts.Battle.Utilities {
    public class MapConfiguration {
        public readonly MapDimensions Dimensions;
        public readonly List<CombatantDatabase.CombatantReference> Combatants;
        public readonly IRandomizer Randomizer;
        public readonly List<Vector2> ObstructedPositions; 

        public MapConfiguration(MapDimensions dimensions, List<CombatantDatabase.CombatantReference> combatants, IRandomizer randomizer, List<Vector2> obstructedPositions) {
            Dimensions = dimensions;
            Combatants = combatants;
            Randomizer = randomizer;
            ObstructedPositions = obstructedPositions;
        }
    }
}