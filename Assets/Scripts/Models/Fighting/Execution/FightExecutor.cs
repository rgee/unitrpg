using Models.Fighting.Skills;

namespace Models.Fighting.Execution {
    public class FightExecutor {
        public void Execute(FightForecast forecast) {
            var initialAttack = forecast.AttackerForecast;
            var initialStrategy = SkillDatabase.Instance.GetStrategyByType(initialAttack.Type);
        } 
    }
}