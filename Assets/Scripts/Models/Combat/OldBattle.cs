using System;
using System.Collections.Generic;
using System.Linq;
using Models.Combat.Inventory;
using Models.Combat.Objectives;
using UnityEngine;

namespace Models.Combat {
    public class OldBattle : IOldBattle {
        private readonly IOldMap _map;
        private readonly IObjective _objective;
        private readonly ITurn _turnState;
        private readonly IActionProber _actionProber;

        public OldBattle(IOldMap map, IObjective objective, ITurn turnState, IActionProber actionProber) {
            _map = map;
            _objective = objective;
            _turnState = turnState;
            _actionProber = actionProber;
        }

        public InteractiveTile GetInteractiveTileByLocation(Vector2 position) {
            return _map.GetTileByPosition(position);
        }

        public IEnumerable<InteractiveTile> GetAdjacentInteractiveTiles(Vector2 position) {
            return _map.GetAdjacentInteractiveTiles(position);
        }

        public void TriggerInteractiveTile(InteractiveTile tile, Unit unit) {
            tile.Trigger();
            _turnState.RecordAction(unit);
        }

        public IEnumerable<CombatAction> GetAvailableActions(Unit unit) {
            return _actionProber.GetAvailableActions(unit);
        }

        public IEnumerable<CombatAction> GetAvailableFightActions(Unit unit) {
            return _actionProber.GetAvailableFightActions(unit);
        } 

        public int TurnCount {
            get { return _turnState.TurnCount; }
        }

        public void EndTurn(TurnControl control) {
            _turnState.End(control);
        }

        public bool CanAct(Unit unit) {
            return _turnState.CanAct(unit);
        }

        public bool CanMove(Unit unit) {
            return GetRemainingMoves(unit) > 0;
        }

        public int GetMovesUsed(Unit unit) {
            return _turnState.GetUsedMoves(unit);
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

        public void AddUnit(Unit unit) {
            _map.AddUnit(unit);
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

        public void UseItem(Item item, Unit unit) {
            unit.UseItem(item);
           _turnState.RecordAction(unit);
        }
    }
}