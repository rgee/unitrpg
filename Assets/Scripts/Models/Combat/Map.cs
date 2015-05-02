using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models.Combat {
    public class Map : IMap {
        private readonly Dictionary<Vector2, Models.Combat.Unit> _unitsByPosition = new Dictionary<Vector2, Models.Combat.Unit>();

        public Map(IEnumerable<Models.Combat.Unit> units) {
            foreach (var unit in units) {
                if (_unitsByPosition.ContainsKey(unit.GridPosition)) {
                    throw new ArgumentException("Cannot place two units at the same position.");
                }

                _unitsByPosition[unit.GridPosition] = unit;
            }
        }

        public IEnumerable<Models.Combat.Unit> GetFriendlyUnits() {
            return from unit in GetAllUnits()
                   where unit.IsFriendly
                   select unit;
        }

        public IEnumerable<Models.Combat.Unit> GetEnemyUnits() {
            return from unit in GetAllUnits()
                   where !unit.IsFriendly
                   select unit;
        }

        public void MoveUnit(Models.Combat.Unit unit, Vector2 location) {
            if (_unitsByPosition[location] != null) {
                throw new ArgumentException("Cannot move to square already occupied: " + location);
            }

            _unitsByPosition.Remove(location);
            unit.GridPosition = location;
            _unitsByPosition[location] = unit;

            CombatEventBus.MoveSignal.Dispatch(unit, location);
        }

        public Models.Combat.Unit GetUnitByPosition(Vector2 position) {
            return _unitsByPosition[position];
        }

        public IEnumerable<Models.Combat.Unit> GetAllUnits() {
            return _unitsByPosition.Values;
        }
    }
}
