using System;
using System.Collections.Generic;
using System.Linq;
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
        private Turn _currentTurn;

        public Battle(IMap map, IRandomizer randomizer, ICombatantDatabase combatants, List<ArmyType> turnOrder) {
            TurnNumber = 0;
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

        public List<ICombatant> GetByArmy(ArmyType army) {
            return _combatants.GetCombatantsByArmy(army);
        }

        public int TurnNumber { get; private set; }

        public ICombatant GetById(string id) {
            if (!_combatantsById.ContainsKey(id)) {
                return null;
            }

            return _combatantsById[id];
        }

        public SkillType GetWeaponSkillForRange(int range) {
            return range > 1 ? SkillType.Ranged : SkillType.Melee;
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
            while (combatants.Count <= 0) {
                var armyIndex = turnCount % _turnOrder.Count;
                var army = _turnOrder[armyIndex];
                combatants = _combatants.GetCombatantsByArmy(army);
                turnCount++;
            }

            _currentTurn = new Turn(combatants);
        }

        public void SubmitAction(ICombatAction action) {
            if (!action.IsValid(_currentTurn)) {
                throw new ArgumentException("Invalid action.");
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