using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Models.Fighting.AI;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using Models.Fighting.Maps.Configuration;
using Models.Fighting.Maps.Triggers;
using Models.Fighting.Skills;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Models.Fighting.Battle {
    public class Battle : IBattle {
        public IMap Map { get; private set; }
        public int TurnNumber { get; private set; }
        public Signal<string> EventTileSignal { get; private set; }
        public List<IObjective> Objectives { get; set; }

        private readonly IRandomizer _randomizer;
        private readonly FightForecaster _forecaster;
        private readonly FightFinalizer _finalizer;
        private readonly List<ArmyType> _turnOrder;
        private readonly ICombatantDatabase _combatants;
        private readonly Dictionary<string, ICombatant> _combatantsById = new Dictionary<string, ICombatant>();
        private Turn _currentTurn;

        public Battle(IMap map, IRandomizer randomizer, ICombatantDatabase combatants, List<ArmyType> turnOrder, List<IObjective> objectives, 
            MapConfig mapConfig) {
            TurnNumber = 0;
            EventTileSignal = new Signal<string>();
            Objectives = objectives;
            Map = map;
            _randomizer = randomizer;
            _combatants = combatants;

            map.EventTileTriggeredSignal.AddListener(_relayEvent);

            var skillDatabase = new SkillDatabase(map);
            _forecaster = new FightForecaster(map, skillDatabase);
            _finalizer = new FightFinalizer(skillDatabase);
            _turnOrder = turnOrder
                .Where(army => combatants.GetCombatantsByArmy(army).Count > 0)
                .ToList();

            foreach (var combatant in combatants.GetAllCombatants()) {
                RegisterCombatant(combatant, map);
            }

            foreach (var eventTile in mapConfig.EventTiles) {
                Map.AddEventTile(eventTile);
            }

            var firstArmy = _turnOrder[0];
            _currentTurn = new Turn(_combatants, firstArmy);
        }

        public IActionPlan GetActionPlan(ArmyType army) {
            var combatants = _combatants.GetCombatantsByArmy(army);
            return new AIActionPlan(combatants);
        }

        public void SpawnCombatant(ICombatant combatant) {
            Map.AddCombatant(combatant);
            RegisterCombatant(combatant, Map);
            _currentTurn.AddNewCombatant(combatant);
        }

        public void AddEventTile(EventTile eventTile) {
            Map.AddEventTile(eventTile);
        }

        public void RemoveEventTile(Vector2 location) {
            Map.RemoveEventTile(location);
        }

        private void RegisterCombatant(ICombatant combatant, IMap map) {
            _combatantsById[combatant.Id] = combatant;
            combatant.MoveSignal.AddListener(destination => map.MoveCombatant(combatant, destination));
            combatant.DeathSignal.AddListener(() => map.RemoveCombatant(combatant));
        }

        public bool IsWon() {
            return Objectives.All(obj => obj.IsComplete(this)) && !Objectives.Any(obj => obj.HasFailed(this));
        }

        public bool IsLost() {
            return Objectives.Any(obj => obj.HasFailed(this));
        }

        private void _relayEvent(EventTile eventTile) {
            EventTileSignal.Dispatch(eventTile.EventName);
        }

        public List<ICombatant> GetAliveByArmy(ArmyType army) {
            return Map.GetAllOnMap()
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

        public BattlePhase NextPhase {
            get {
                var index = _turnOrder.IndexOf(_currentTurn.Army) + 1;
                var seenArmies = 0;

                while (true) {
                    var armyIndex = index % _turnOrder.Count;
                    var army = _turnOrder[armyIndex];
                    if (_combatants.GetCombatantsByArmy(army).Count > 0) {
                        return army.GetBattlePhase();
                    }

                    if (seenArmies >= Enum.GetNames(typeof(ArmyType)).Length) {
                        throw new Exception("No eligible next battle phase.");
                    }

                    index++;
                    seenArmies++;
                }
            }
        }

        public void EndTurn() {
            var nextArmy = NextPhase.GetArmyType();

            TurnNumber++;
            _currentTurn = new Turn(_combatants, nextArmy);
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