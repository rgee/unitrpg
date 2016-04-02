using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Fighting.Maps {
    public class Map : IMap {
        private readonly Dictionary<Vector2, ICombatant> _combatantsByPosition = new Dictionary<Vector2, ICombatant>();

        public Map() {
            CombatEventBus.CombatantMoves.AddListener(MoveCombatant);
        }

        public void AddCombatant(ICombatant combatant) {
            var position = combatant.Position;
            if (_combatantsByPosition.ContainsKey(position)) {
                throw new ArgumentException("There's already a combatant at "+ position);
            }
            _combatantsByPosition[position] = combatant;
        }

        public void MoveCombatant(ICombatant combatant, Vector2 position) {
            if (_combatantsByPosition.ContainsKey(position)) {
                throw new ArgumentException("There's already a combatant at "+ position);
            }

            _combatantsByPosition.Remove(combatant.Position);
            _combatantsByPosition[position] = combatant;
            combatant.Position = position;
        }

        public ICombatant GetAtPosition(Vector2 position) {
            ICombatant result;
            _combatantsByPosition.TryGetValue(position, out result);
            return result;
        }
    }
}