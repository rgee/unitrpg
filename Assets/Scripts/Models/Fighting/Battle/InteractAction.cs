using Models.Fighting.Maps;
using Models.Fighting.Maps.Triggers;

namespace Models.Fighting.Battle {
    public class InteractAction : ICombatAction {
        public readonly EventTile Tile;
        private readonly ICombatant _combatant;
        private readonly IMap _map;

        public InteractAction(EventTile tile, ICombatant combatant, IMap map) {
            Tile = tile;
            _combatant = combatant;
            _map = map;
        }

        public string GetValidationError(Turn turn) {
            if (!turn.CanAct(_combatant)) {
                return "The attacker, " + _combatant.Id + " has already acted this turn.";
            }

            return null;
        }

        public void Perform(Turn turn) {
            turn.MarkAction(_combatant);
            if (Tile.OneTimeUse) {
                _map.RemoveEventTile(Tile.Location);
            }
        }

        public IPointOfInterest GetPointofInterest(ICombatActionVisitor visitor) {
            return visitor.Visit(this);
        }
    }
}