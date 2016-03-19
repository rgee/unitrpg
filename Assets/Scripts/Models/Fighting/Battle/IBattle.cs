using Models.Fighting.Execution;
using Models.Fighting.Skills;

namespace Models.Fighting.Battle {
    public interface IBattle {
        void SubmitAction(ICombatAction action);

        void EndTurn();

        bool CanMove(ICombatant combatant);

        bool CanAct(ICombatant combatant);

        FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type);
        FinalizedFight FinalizeFight(FightForecast forecast);
    }
}