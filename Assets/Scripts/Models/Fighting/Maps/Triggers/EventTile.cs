using UnityEngine;

namespace Models.Fighting.Maps.Triggers {
    /// <summary>
    /// Event Tiles represent a location on the map where an event can occur
    /// if a friendly unit interacts with this tile somehow.
    /// </summary>
    public class EventTile {
        public readonly Vector2 Location;
        public readonly string EventName;
        public readonly bool OneTimeUse;
        public readonly InteractionMode Type;

        public EventTile(Vector2 location, string eventName, bool oneTimeUse, InteractionMode type) {
            Location = location;
            EventName = eventName;
            OneTimeUse = oneTimeUse;
            Type = type;
        }
    }
}