using System.Collections.Generic;
using Models.Fighting.Maps.Triggers;

namespace Models.Fighting.Maps.Configuration {
    public class MapConfig {
        public readonly List<EventTile> EventTiles;

        public MapConfig(List<EventTile> eventTiles) {
            EventTiles = eventTiles;
        }
    }
}