using Models.Fighting.Maps;
using UnityEngine;

namespace Models.Fighting.Battle {
    public class MoveAction : ICombatAction {
        private readonly IMap _map;
        private readonly ICombatant _combatant;
        private readonly Vector2 _destination;
        private readonly int _pathLength;

        public MoveAction(IMap map, ICombatant combatant, Vector2 destination, int pathLength) {
            _map = map;
            _combatant = combatant;
            _destination = destination;
            _pathLength = pathLength;
        }

        public MoveAction(IMap map, ICombatant combatant, Vector2 destination) {
            _map = map;
            _combatant = combatant;
            _destination = destination;
            _pathLength = MathUtils.ManhattanDistance(combatant.Position, destination);
        }


        public bool IsValid(Turn turn) {
            return turn.GetRemainingMoveDistance(_combatant) <= _pathLength;
        }

        public void Perform(Turn turn) {
            _map.MoveCombatant(_combatant, _destination);
            turn.MarkMove(_combatant, _pathLength);
        }
    }
}