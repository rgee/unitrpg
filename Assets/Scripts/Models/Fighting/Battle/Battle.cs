using System;
using System.Collections.Generic;
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
        private readonly ICombatantDatabase _combatants;
        private int _turnNumber = 1;
        private Turn _currentTurn;

        public Battle(IMap map, IRandomizer randomizer, ICombatantDatabase combatants, List<ArmyType> turnOrder) {
            _randomizer = randomizer;
            _combatants = combatants;
            _forecaster = new FightForecaster(map);
            _finalizer = new FightFinalizer();
            _turnOrder = turnOrder;

            var firstArmy = _turnOrder[0];
            var firstCombatants = _combatants.GetCombatantsByArmy(firstArmy);
            _currentTurn = new Turn(firstCombatants);
        }

        public void EndTurn() {
            var armyIndex = _turnNumber % _turnOrder.Count;
            var army = _turnOrder[armyIndex];
            var combatants = _combatants.GetCombatantsByArmy(army);
            _currentTurn = new Turn(combatants);
        }

        public void SubmitAction(ICombatAction action) {
            if (!action.IsValid(_currentTurn)) {
                throw new ArgumentException("Invalid action.");
            }

            action.Perform(_currentTurn);
        }

        public FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type) {
            return _forecaster.Forecast(attacker, defender, type);
        }

        public FinalizedFight FinalizeFight(FightForecast forecast) {
            return _finalizer.Finalize(forecast, _randomizer);
        }
    }
}