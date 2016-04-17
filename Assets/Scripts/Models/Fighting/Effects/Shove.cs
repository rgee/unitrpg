using Models.Fighting.Maps;

namespace Models.Fighting.Effects {
    public class Shove : IEffect {
        private readonly MathUtils.CardinalDirection _direction;
        private readonly IMap _map;

        public Shove(MathUtils.CardinalDirection direction, IMap map) {
            _direction = direction;
            _map = map;
        }

        public void Apply(ICombatant combatant) {
            var position = combatant.Position;
            var destination = MathUtils.GetAdjacentPoint(position, _direction);
            if (!_map.IsBlocked(destination)) {
                combatant.MoveTo(destination);
            }
        }
    }
}