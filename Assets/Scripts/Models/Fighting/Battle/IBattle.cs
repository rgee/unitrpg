using Models.Fighting.Execution;
using Models.Fighting.Skills;

namespace Models.Fighting.Battle {
    public interface IBattle {
        bool ShouldTurnEnd();

        int GetRemainingMoves(ICombatant combatant);

        void SubmitAction(ICombatAction action);

        void EndTurn();

        bool CanMove(ICombatant combatant);

        bool CanAct(ICombatant combatant);

        FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type);

        FinalizedFight FinalizeFight(FightForecast forecast);

        int GetMaxWeaponAttackRange(ICombatant combatant);

        SkillType GetWeaponSkillForRange(int range);
    }
}