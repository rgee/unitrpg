﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using Models.Fighting.Maps.Configuration;
using Models.Fighting.Maps.Triggers;
using Models.Fighting.Skills;
using strange.extensions.signal.impl;
using UnityEngine;
using UnityEngine.Networking.Match;
using FightExecutor = Models.Fighting.Execution.FightExecutor;

namespace Models.Fighting.Battle {
    public class Battle : IBattle {
        public int TurnNumber { get; private set; }
        public Signal<string> EventTileSignal { get; private set; }

        private readonly Execution.FightExecutor _executor = new Execution.FightExecutor();
        private readonly IRandomizer _randomizer;
        private readonly FightForecaster _forecaster;
        private readonly FightFinalizer _finalizer;
        private readonly List<ArmyType> _turnOrder;
        private readonly ICombatantDatabase _combatants;
        private readonly Dictionary<string, ICombatant> _combatantsById = new Dictionary<string, ICombatant>();
        private readonly IMap _map;
        private readonly List<IObjective> _objectives;
        private Turn _currentTurn;

        public Battle(IMap map, IRandomizer randomizer, ICombatantDatabase combatants, List<ArmyType> turnOrder) : 
            this(map, randomizer, combatants,turnOrder, new List<IObjective> { new Rout() }, new StaticMapConfigRepository(), null) {
        }

        public Battle(IMap map, IRandomizer randomizer, ICombatantDatabase combatants, List<ArmyType> turnOrder, List<IObjective> objectives, 
            IMapConfigRepository mapConfigRepository, string mapName) {
            TurnNumber = 0;
            EventTileSignal = new Signal<string>();
            _objectives = objectives;
            _map = map;
            _randomizer = randomizer;
            _combatants = combatants;

            var skillDatabase = new SkillDatabase(map);
            _forecaster = new FightForecaster(map, skillDatabase);
            _finalizer = new FightFinalizer(skillDatabase);
            _turnOrder = turnOrder;

            foreach (var combatant in combatants.GetAllCombatants()) {
                RegisterCombatant(combatant, map);
            }

            // If there's a map config in the database for this map, use it.
            if (mapName != null) {
                var config = mapConfigRepository.GetConfigByMapName(mapName);
                if (config == null) {
                    Debug.LogErrorFormat("Could not find map config for {0}", mapName);
                } else {
                    foreach (var eventTile in config.EventTiles) {
                        _map.AddEventTile(eventTile);
                    }
                }
            }

            var firstArmy = _turnOrder[TurnNumber];
            var firstCombatants = _combatants.GetCombatantsByArmy(firstArmy);
            _currentTurn = new Turn(firstCombatants);
        }


        public void AddEventTile(EventTile eventTile) {
            _map.AddEventTile(eventTile);
        }

        public void RemoveEventTile(Vector2 location) {
            _map.RemoveEventTile(location);
        }

        private void RegisterCombatant(ICombatant combatant, IMap map) {
            _combatantsById[combatant.Id] = combatant;
            combatant.MoveSignal.AddListener(destination => map.MoveCombatant(combatant, destination));
            combatant.DeathSignal.AddListener(() => map.RemoveCombatant(combatant));
        }

        public List<IObjective> GetObjectives() {
            return _objectives;
        }

        public bool IsWon() {
            return _objectives.All(obj => obj.IsComplete(this)) && !_objectives.Any(obj => obj.HasFailed(this));
        }

        public bool IsLost() {
            return _objectives.Any(obj => obj.HasFailed(this));
        }

        public void MoveCombatant(ICombatant combatant, List<Vector2> path) {
            _validateMove(combatant, path);

            _map.MoveCombatant(combatant, path.Last());
            _currentTurn.MarkMove(combatant, path.Count);
            _processTriggers(path);
        }

        private void _processTriggers(IEnumerable<Vector2> path) {
            foreach (var tile in path) {
                var eventTile = _map.GetEventTile(tile);
                if (eventTile != null && eventTile.InteractionMode == InteractionMode.Walk) {
                    Debug.Log("Dispatching event tile trigger event: " + eventTile.EventName);
                    EventTileSignal.Dispatch(eventTile.EventName);
                    if (eventTile.OneTimeUse) {
                        _map.RemoveEventTile(tile);
                    }
                }
            }
        }

        private void _validateMove(ICombatant combatant, List<Vector2> path) {
            if (_currentTurn.GetRemainingMoveDistance(combatant) < path.Count) {
               throw new InvalidActionException(combatant.Id + " has already moved this turn.");
            }

            foreach (var location in path) {
                if (location != combatant.Position && _map.IsBlocked(location)) {
                    var error = string.Format("Location ({0}, {1}) is blocked.", location.x, location.y);
                    throw new InvalidActionException(error);
                }
            }
        }

        public List<ICombatant> GetAliveByArmy(ArmyType army) {
            return _map.GetAllOnMap()
                .Where(combatant => combatant.Army == army)
                .ToList();
        }

        public ICombatant GetById(string id) {
            if (!_combatantsById.ContainsKey(id)) {
                return null;
            }

            return _combatantsById[id];
        }

        public SkillType GetWeaponSkillForRange(ICombatant attacker, int range) {
            var weapons = attacker.EquippedWeapons;
            if (weapons.Any(weapon => weapon.Range > 1)) {
                return range > 1 ? SkillType.Ranged : SkillType.Melee;
            }

            return SkillType.Melee;
        }

        public void ExecuteFight(FinalizedFight fight) {
            _executor.Execute(fight);
            _currentTurn.MarkAction(fight.InitialPhase.Initiator);
        }

        public int GetMaxWeaponAttackRange(ICombatant combatant) {
            var weapons = combatant.EquippedWeapons;
            return weapons.Max(weapon => weapon.Range);
        }

        public bool ShouldTurnEnd() {
            return _currentTurn.ShouldTurnEnd();
        }

        public int GetRemainingMoves(ICombatant combatant) {
            return _currentTurn.GetRemainingMoveDistance(combatant);
        }

        public void EndTurn() {
            var combatants = new List<ICombatant>();
            var turnCount = TurnNumber;
            var seenArmies = new HashSet<ArmyType>();

            while (combatants.Count <= 0) {
                var armyIndex = turnCount % _turnOrder.Count;
                var army = _turnOrder[armyIndex];
                seenArmies.Add(army);
                combatants = _combatants.GetCombatantsByArmy(army);
                turnCount++;

                if (seenArmies.Contains(army)) {
                    break;
                }
            }

            _currentTurn = new Turn(combatants);
        }

        public void SubmitAction(ICombatAction action) {
            var validationError = action.GetValidationError(_currentTurn);
            if (validationError != null) {
                throw new InvalidActionException(validationError);
            }

            action.Perform(_currentTurn);
        }

        public bool CanMove(ICombatant combatant) {
            return _currentTurn.CanMove(combatant);
        }

        public bool CanAct(ICombatant combatant) {
            return _currentTurn.CanAct(combatant);
        }

        public FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type) {
            return _forecaster.Forecast(attacker, defender, type);
        }

        public FinalizedFight FinalizeFight(FightForecast forecast) {
            return _finalizer.Finalize(forecast, _randomizer);
        }
    }
}