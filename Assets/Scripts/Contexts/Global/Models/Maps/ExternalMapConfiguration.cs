using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Contexts.Global.Models;
using Models.Fighting.Maps.Triggers;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Contexts.Global.Models {
    public class ExternalMapConfiguration {
        public readonly string DisplayName;
        public readonly int Width;
        public readonly int Height;
        public readonly List<Vector2> ObstructionLocations;
        public readonly List<TurnEventConfiguration> TurnEvents;
        public readonly List<TriggerTileConfiguration> TriggerTiles;

        public ExternalMapConfiguration(string displayName, int width, int height, List<Vector2> obstructionLocations, List<TurnEventConfiguration> turnEvents, List<TriggerTileConfiguration> triggerTiles) {
            DisplayName = displayName;
            Width = width;
            Height = height;
            ObstructionLocations = obstructionLocations;
            TurnEvents = turnEvents;
            TriggerTiles = triggerTiles;
        }

        public static ExternalMapConfiguration CreateFromJson(JObject json) {
            var displayName = json["displayName"].ToObject<string>();
            var width = json["width"].ToObject<int>();
            var height = json["height"].ToObject<int>();

            var obstructionsNode = json["obstructions"] as JObject;
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
                        var locationNode = tile["position"] as JObject;
                        var location = new Vector2(locationNode["x"].ToObject<int>(), locationNode["y"].ToObject<int>());

                        var name = tile["eventName"].ToObject<string>();
                        var mode = tile["interactionMode"].ToObject<string>();

                        return new TriggerTileConfiguration(location, name, ParseInteractionMode(mode));
                    })
                    .ToList();
            }

            return new ExternalMapConfiguration(displayName, width, height, obstructionValues, turnEvents, triggerTiles);
        }

        private static InteractionMode ParseInteractionMode(string modeString) {
            if (string.IsNullOrEmpty(modeString)) {
                throw new ArgumentException("An interactino mode is required.");
            }

            switch (modeString) {
                case "walk":
                    return InteractionMode.Walk;
                case "use":
                    return InteractionMode.Use;
                default:
                    throw new ArgumentException("Invalid interaction mode: " + modeString);
            }
        }
    }
}