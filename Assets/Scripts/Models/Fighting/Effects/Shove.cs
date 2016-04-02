namespace Models.Fighting.Effects {
    public class Shove : IEffect {
        private readonly MathUtils.CardinalDirection _direction;

        public Shove(MathUtils.CardinalDirection direction) {
            _direction = direction;
        }

        public void Apply(ICombatant combatant) {
            var position = combatant.Position;
            var destination = MathUtils.GetAdjacentPoint(position, _direction);
            combatant.MoveTo(destination);
        }
    }
}