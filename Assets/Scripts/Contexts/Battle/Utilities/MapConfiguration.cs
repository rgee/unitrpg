using System.Collections.Generic;
using Models.Fighting;
using Models.Fighting.Battle;

namespace Contexts.Battle.Utilities {
    public class MapConfiguration {
        public readonly MapDimensions Dimensions;
        public readonly List<CombatantDatabase.CombatantReference> Combatants;
        public readonly IRandomizer Randomizer;

        public MapConfiguration(MapDimensions dimensions, List<CombatantDatabase.CombatantReference> combatants, IRandomizer randomizer) {
            Dimensions = dimensions;
            Combatants = combatants;
            Randomizer = randomizer;
        }
    }
}