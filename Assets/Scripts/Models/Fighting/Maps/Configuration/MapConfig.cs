using System.Collections.Generic;
using Models.Fighting.Maps.Triggers;

namespace Models.Fighting.Maps.Configuration {
    public class MapConfig {
        public readonly List<EventTile> EventTiles;
        public readonly List<TurnEvent> TurnEvents;

        public MapConfig(List<EventTile> eventTiles, List<TurnEvent> turnEvents) {
            EventTiles = eventTiles;
            TurnEvents = turnEvents;
        }
    }
}