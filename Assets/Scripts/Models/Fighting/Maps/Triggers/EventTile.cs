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
        public readonly InteractionMode InteractionMode;

        public EventTile(Vector2 location, string eventName, bool oneTimeUse, InteractionMode interactionMode) {
            Location = location;
            EventName = eventName;
            OneTimeUse = oneTimeUse;
            InteractionMode = interactionMode;
        }
    }
}