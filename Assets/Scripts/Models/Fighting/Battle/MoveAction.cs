using System.Collections.Generic;
using Models.Fighting.Maps;
using Models.Fighting.Maps.Triggers;
using UnityEngine;
using Utils;

namespace Models.Fighting.Battle {
    public class MoveAction : ICombatAction {
        public readonly List<Vector2> Path;
        public readonly ICombatant Combatant;

        private readonly IMap _map;
        private readonly Vector2 _destination;
        private readonly int _pathLength;

        public MoveAction(IMap map, ICombatant combatant, Vector2 destination, List<Vector2> path) {
            _map = map;
            Combatant = combatant;
            _destination = destination;
            Path = path;
            _pathLength = Path.Count;
        }

        public MoveAction(IMap map, ICombatant combatant, Vector2 destination) {
            _map = map;
            Combatant = combatant;
            _destination = destination;
            _pathLength = MathUtils.ManhattanDistance(combatant.Position, destination);
        }


        public string GetValidationError(Turn turn) {
            if (turn.GetRemainingMoveDistance(Combatant) < _pathLength) {
                return "The attacker, " + Combatant.Id + " has already moved this turn.";
            }

            return null;
        }

        public void Perform(Turn turn) {
            Debug.LogFormat("Moving Combatant {0}({1}) to {2}", Combatant.Name, Combatant.Id, _destination);
            _map.MoveCombatant(Combatant, _destination);
            turn.MarkMove(Combatant, _pathLength);

            if (Path != null) {
                foreach (var location in Path) {
                    var eventTile = _map.GetEventTile(location);
                    if (eventTile != null && eventTile.InteractionMode == InteractionMode.Walk) {
                        _map.TriggerEventTile(location);
                    }
                }                
            }
        }
    }
}