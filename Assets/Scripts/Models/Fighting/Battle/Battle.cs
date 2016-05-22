using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using Models.Fighting.Skills;

namespace Models.Fighting.Battle {
    public class Battle : IBattle {
        private readonly IRandomizer _randomizer;
        private readonly FightForecaster _forecaster;
        private readonly FightFinalizer _finalizer;
        private readonly List<ArmyType> _turnOrder;
        private readonly SkillDatabase _skillDatabase;
        private readonly ICombatantDatabase _combatants;
        private readonly Dictionary<string, ICombatant> _combatantsById = new Dictionary<string, ICombatant>();
        private readonly IMap _map;
        private readonly List<IObjective> _objectives;
        private Turn _currentTurn;

        public Battle(IMap map, IRandomizer randomizer, ICombatantDatabase combatants, List<ArmyType> turnOrder) : 
            this(map, randomizer, combatants,turnOrder, new List<IObjective> { new Rout() }) {
        }

        public Battle(IMap map, IRandomizer randomizer, ICombatantDatabase combatants, List<ArmyType> turnOrder, List<IObjective> objectives) {
            TurnNumber = 0;
            _objectives = objectives;
            _map = map;
            _randomizer = randomizer;
            _combatants = combatants;
            _skillDatabase = new SkillDatabase(map);
            _forecaster = new FightForecaster(map, _skillDatabase);
            _finalizer = new FightFinalizer(_skillDatabase);
            _turnOrder = turnOrder;

            foreach (var combatant in combatants.GetAllCombatants()) {
                _combatantsById[combatant.Id] = combatant;
            }

            var firstArmy = _turnOrder[TurnNumber];
            var firstCombatants = _combatants.GetCombatantsByArmy(firstArmy);
            _currentTurn = new Turn(firstCombatants);
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

        public List<ICombatant> GetAliveByArmy(ArmyType army) {
            return _map.GetAllOnMap()
                .Where(combatant => combatant.Army == army)
                .ToList();
        }

        public int TurnNumber { get; private set; }

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
                throw new ArgumentException("Invalid action [" + action.GetType() + "]: " + validationError);
            }

            action.Perform(_currentTurn);

            foreach (var combatant in _combatants.GetAllCombatants()) {
                if (!combatant.IsAlive) {
                    _map.RemoveCombatant(combatant);
                }
            }
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