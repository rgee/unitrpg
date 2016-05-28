using System.Collections.Generic;
using Models.Fighting.Maps;
using UnityEngine;

namespace Models.Fighting.Battle {
    public class MoveAction : ICombatAction {
        private readonly IMap _map;
        private readonly ICombatant _combatant;
        private readonly Vector2 _destination;
        private readonly int _pathLength;
        private readonly List<Vector2> _path;

        public MoveAction(IMap map, ICombatant combatant, Vector2 destination, List<Vector2> path) {
            _map = map;
            _combatant = combatant;
            _destination = destination;
            _path = path;
            _pathLength = _path.Count;
        }

        public MoveAction(IMap map, ICombatant combatant, Vector2 destination) {
            _map = map;
            _combatant = combatant;
            _destination = destination;
            _pathLength = MathUtils.ManhattanDistance(combatant.Position, destination);
        }


        public string GetValidationError(Turn turn) {
            if (turn.GetRemainingMoveDistance(_combatant) < _pathLength) {
                return "The attacker, " + _combatant.Id + " has already moved this turn.";
            }

            return null;
        }

        public void Perform(Turn turn) {
            _map.MoveCombatant(_combatant, _destination);
            turn.MarkMove(_combatant, _pathLength);

            if (_path != null) {
                foreach (var tile in _path) {
                    var eventTile = _map.GetEventTile(tile);
                    if (eventTile != null) {
                        
                    }
                }                
            }
        }
    }
}