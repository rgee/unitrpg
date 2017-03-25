using Contexts.Battle.Utilities;
using Models.Fighting.Battle;

namespace Contexts.Battle.Models {
    public class PointOfInterestFactory : ICombatActionVisitor {
        private readonly MapDimensions _dimensions;

        public PointOfInterestFactory(MapDimensions dimensions) {
            _dimensions = dimensions;
        }

        public IPointOfInterest Visit(FightAction fight) {
            var attacker = fight.Fight.InitialPhase.Initiator;
            var defender = fight.Fight.InitialPhase.Receiver;
            return new FightPointOfInterest(_dimensions, attacker, defender);
        }

        public IPointOfInterest Visit(MoveAction move) {
            return new CombatantPointOfInterest(_dimensions, move.Combatant);
        }

        public IPointOfInterest Visit(UseItemAction useItem) {
            return new CombatantPointOfInterest(_dimensions, useItem.Combatant);
        }

        public IPointOfInterest Visit(InteractAction interact) {
            return new TilePointOfInterest(_dimensions, interact.Tile.Location);
        }
    }
}