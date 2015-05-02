using System;
using System.Collections.Generic;
using System.Linq;
using Models.Combat.Objectives;
using UnityEngine;

namespace Models.Combat {
    public class Battle : IBattle {
        private readonly IMap _map;
        private readonly IObjective _objective;
        private readonly ITurn _turnState;

        public Battle(IMap map, IObjective objective, ITurn turnState) {
            _map = map;
            _objective = objective;
            _turnState = turnState;
        }

        public int TurnCount { get; private set; }

        public bool CanAct(Unit unit) {
            return _turnState.CanAct(unit);
        }

        public int GetRemainingMoves(Unit unit) {
            return _turnState.GetRemainingMoves(unit);
        }

        public void WaitUnit(Unit unit) {
            _turnState.RecordAction(unit);
        }

        public bool IsFailed() {
            return _objective.IsFailed(this);
        }

        public bool IsComplete() {
            return _objective.IsComplete(this);
        }

        public Unit GetUnitByName(string name) {
            return _map.GetAllUnits()
                       .First(unit => unit.Character.Name == name);
        }

        public Unit GetUnitByLocation(Vector2 position) {
            return _map.GetUnitByPosition(position);
        }

        public void MoveUnit(Unit unit, List<Vector2> path, Vector2 location) {
            _map.MoveUnit(unit, location);
            _turnState.RecordMove(unit, path.Count);
        }

        public IEnumerable<Unit> GetFriendlyUnits() {
            return from unit in _map.GetAllUnits()
                   where unit.IsFriendly
                   select unit;
        }

        public IEnumerable<Unit> GetEnemyUnits() {
            return from unit in _map.GetAllUnits()
                   where !unit.IsFriendly
                   select unit;
        }

        public Fight SimulateFight(Unit attacker, AttackType attack, Unit defender) {
            throw new NotImplementedException();
        }

        public void ExecuteFight(Fight fight) {
            _turnState.RecordAction(fight.Participants.Attacker);
        }
    }
}