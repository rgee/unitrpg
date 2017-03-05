using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Maps.Configuration;
using Models.Fighting.Maps.Triggers;

namespace Contexts.Global.Models {
    public class ExternalMapConfigurationRepository : IMapConfigRepository {
        private readonly Dictionary<string, ExternalMapConfiguration> _configs;

        public ExternalMapConfigurationRepository(Dictionary<string, ExternalMapConfiguration> configs) {
            _configs = configs;
        }

        public MapConfig GetConfigByMapName(string mapName) {
            var externalConfig = _configs[mapName];
            var eventTiles = externalConfig.TriggerTiles.Select(tile => {
                    // TODO: Get one-time-use / repeatable config from external
                    return new EventTile(tile.Location, tile.EventName, true, tile.InteractionMode);
                })
                .ToList();
            var turnEvents = externalConfig.TurnEvents.Select(turnEvent => new TurnEvent(turnEvent.Turn, turnEvent.EventName))
                .ToList();
            return new MapConfig(eventTiles, turnEvents);
        }
    }
}