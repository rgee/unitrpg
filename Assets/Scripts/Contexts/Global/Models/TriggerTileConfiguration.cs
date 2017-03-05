using Models.Fighting.Maps.Triggers;
using UnityEngine;

namespace Contexts.Global.Models {
    public class TriggerTileConfiguration {
        public readonly Vector2 Location;
        public readonly string EventName;
        public readonly InteractionMode InteractionMode;

        public TriggerTileConfiguration(Vector2 location, string eventName, InteractionMode interactionMode) {
            Location = location;
            EventName = eventName;
            InteractionMode = interactionMode;
        }
    }
}