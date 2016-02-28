using Models.Fighting.Skills;

namespace Models.Fighting.Execution {
    public class FightForecaster {
        public FightForecast Forecast(ICombatant attacker, ICombatant defender, SkillType type) {
            var strategy = SkillDatabase.Instance.GetStrategyByType(type);

            return null;
        }
    }
}