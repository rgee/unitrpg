using System;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using Models.Fighting.Skills;

namespace Models.Fighting.Battle {
    public class Battle : IBattle {
        private readonly IMap _map;
        private readonly IRandomizer _randomizer;
        private readonly FightForecaster _forecaster;
        private readonly FightFinalizer _finalizer;
        private Turn _currentTurn;

        public Battle(IMap map, IRandomizer randomizer) {
            _map = map;
            _randomizer = randomizer;
            _forecaster = new FightForecaster(_map);
            _finalizer = new FightFinalizer();
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