using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Contexts.Global.Models;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Contexts.Global.Models {
    public class MapConfiguration {
        public readonly int Width;
        public readonly int Height;
        public readonly List<Vector2> ObstructionLocations;
        public readonly List<TurnEventConfiguration> TurnEvents;
        public readonly List<TriggerTileConfiguration> TriggerTiles;

        public MapConfiguration(int width, int height, List<Vector2> obstructionLocations, List<TurnEventConfiguration> turnEvents, List<TriggerTileConfiguration> triggerTiles) {
            Width = width;
            Height = height;
            ObstructionLocations = obstructionLocations;
            TurnEvents = turnEvents;
            TriggerTiles = triggerTiles;
        }

        public static MapConfiguration CreateFromJson(JObject json) {
            var width = json["width"].ToObject<int>();
            var height = json["height"].ToObject<int>();

            var obstructionsNode = json["obstrutions"] as JObject;
            var obstructionValues = obstructionsNode == null
                ? new List<Vector2>()
                : obstructionsNode
                    .Properties()
                    .Select(prop => new Vector2(prop.Value["x"].ToObject<int>(), prop.Value["y"].ToObject<int>()))
                    .ToList();

            var turnEventsNode = json["turnEvents"] as JObject;
            var turnEvents = new List<TurnEventConfiguration>();
            if (turnEventsNode != null) {
                turnEvents = turnEventsNode
                    .Properties()
                    .Select(prop => {
                        var evt = prop.Value;
                        var turn = evt["turn"].ToObject<int>();
                        var name = evt["eventName"].ToObject<string>();
                        return new TurnEventConfiguration(turn, name);
                    })
                    .ToList();
            }

            var triggerTileNode = json["triggerTiles"] as JObject;
            var triggerTiles = new List<TriggerTileConfiguration>();
            if (triggerTileNode != null) {
                triggerTiles = triggerTileNode
                    .Properties()
                    .Select(prop => {
                        var tile = prop.Value;
                        var locationNode = tile["location"] as JObject;
                        var location =
                            new Vector2(locationNode["x"].ToObject<int>(), locationNode["y"].ToObject<int>());

                        var name = tile["name"].ToObject<string>();

                        return new TriggerTileConfiguration(location, name);
                    })
                    .ToList();
            }

            return new MapConfiguration(width, height, obstructionValues, turnEvents, triggerTiles);
        }
    }
}