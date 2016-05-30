using Models.Fighting.Maps;
using UnityEngine;
using Utils;

namespace Models.Fighting.Effects {
    public class Shove : IEffect {
        private readonly MathUtils.CardinalDirection _direction;
        private readonly IMap _map;

        public Shove(MathUtils.CardinalDirection direction, IMap map) {
            _direction = direction;
            _map = map;
        }

        public Vector2 GetDestination(ICombatant combatant) {
            var position = combatant.Position;
            var destination = MathUtils.GetAdjacentPoint(position, _direction);

            if (_map.IsBlocked(destination)) {
                destination = position;
            }

            return destination;
        }

        public void Apply(ICombatant combatant) {
            var destination = GetDestination(combatant);
            if (!destination.Equals(combatant.Position)) {
                combatant.MoveTo(destination);
            }
        }
    }
}