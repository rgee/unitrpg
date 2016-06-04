using System.Collections.Generic;
using Models.Fighting.Maps.Triggers;
using UnityEngine;

namespace Models.Fighting.Maps.Configuration {
    public class StaticMapConfigRepository : IMapConfigRepository {
        public MapConfig GetConfigByMapName(string mapName) {
            switch (mapName) {
                case "test_events":
                    var tiles = new List<EventTile> {
                        new EventTile(new Vector2(26, 11), "test", true, InteractionMode.Walk)
                    };

                    return new MapConfig(tiles);
                case "chapter2":
                    var chapter2Tiles = new List<EventTile> {
                        new EventTile(new Vector2(16, 19), "inspect_inn", true, InteractionMode.Walk),
                        new EventTile(new Vector2(25, 19), "inspect_house", true, InteractionMode.Walk)
                    };

                    return new MapConfig(chapter2Tiles);
                default:
                    return null;
            }
        }
    }
}