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

        public MapConfiguration(int width, int height, List<Vector2> obstructionLocations, List<TurnEventConfiguration> turnEvents) {
            Width = width;
            Height = height;
            ObstructionLocations = obstructionLocations;
            TurnEvents = turnEvents;
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
            List<TurnEventConfiguration> turnEvents = new List<TurnEventConfiguration>();
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

            return new MapConfiguration(width, height, obstructionValues, turnEvents);
        }
    }
}