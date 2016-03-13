using Models.Fighting.Execution;
using Models.Fighting.Skills;

namespace Models.Fighting.Battle {
    public interface IBattle {
        void SubmitAction(ICombatAction action);

        FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type);
        FinalizedFight FinalizeFight(FightForecast forecast);
    }
}