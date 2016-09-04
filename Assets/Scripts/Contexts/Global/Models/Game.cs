using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Signals;
using Newtonsoft.Json.Linq;

namespace Contexts.Global.Models {
    public class Game {

        public readonly Dictionary<string, MapConfiguration> Maps;

        public Game(Dictionary<string, MapConfiguration> maps) {
            Maps = maps;
        }

        public static Game CreateFromJson(JObject parsedJSON) {

            var mapsObject = parsedJSON["maps"] as JObject;
            var mapIds = mapsObject.Properties().Select(p => p.Name).ToList();
            var maps = mapIds.ToDictionary(id => id, id => {
                var map = mapsObject[id] as JObject;
                return MapConfiguration.CreateFromJson(map);
            });
            
            return new Game(maps);
        } 
    }
}