using System;

namespace Models.Fighting.Maps {
    public class Tile {
        private ICombatant _occupant;
        public ICombatant Occupant {
            get { return _occupant; }
            set {
                if (Obstructed) {
                    throw new ArgumentException("Cannot add an occupant to an obstructed tile.");
                }
                _occupant = value;
            }
        }

        public bool Obstructed { get; set; }
    }
}