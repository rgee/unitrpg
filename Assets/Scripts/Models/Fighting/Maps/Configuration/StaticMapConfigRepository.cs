using System.Collections.Generic;
using Models.Fighting.Maps.Triggers;
using UnityEngine;

namespace Models.Fighting.Maps.Configuration {
    public class StaticMapConfigRepository : IMapConfigRepository {
        public MapConfig GetConfigByMapName(string mapName) {
            switch (mapName) {
                case "test_events":
                    var tiles = new List<EventTile> {
                        new EventTile(new Vector2(28, 11), "test", true, InteractionMode.Walk)
                    };

                    return new MapConfig(tiles);
                default:
                    return null;
            }
        }
    }
}