using System.Collections.Generic;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Maps;
using UnityEngine;

namespace Contexts.Battle.Utilities {
    public class MapConfiguration {
        public readonly MapDimensions Dimensions;
        public readonly IMap Map;
        public readonly ICombatantDatabase Combatants;

        public MapConfiguration(MapDimensions dimensions, IMap map, ICombatantDatabase combatantDatabase) {
            Dimensions = dimensions;
            Map = map;
            Combatants = combatantDatabase;
        }
    }
}