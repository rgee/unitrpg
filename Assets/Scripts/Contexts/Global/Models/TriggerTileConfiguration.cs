using UnityEngine;

namespace Contexts.Global.Models {
    public class TriggerTileConfiguration {
        public readonly Vector2 Location;
        public readonly string EventName;

        public TriggerTileConfiguration(Vector2 location, string eventName) {
            Location = location;
            EventName = eventName;
        }
    }
}