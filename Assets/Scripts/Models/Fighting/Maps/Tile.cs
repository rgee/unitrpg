using System;
using Models.Fighting.Maps.Triggers;

namespace Models.Fighting.Maps {
    public class Tile {
        private ICombatant _occupant;
        public ICombatant Occupant {
            get { return _occupant; }
            set {
                if (Obstructed) {
                    throw new ArgumentException("Cannot add an occupant to an obstructed tile. Id: " + value.Id);
                }
                _occupant = value;
            }
        }

        public EventTile Event;

        public bool Obstructed { get; set; }
    }
}