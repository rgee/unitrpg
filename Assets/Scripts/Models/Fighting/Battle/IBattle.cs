using System.Collections.Generic;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Skills;

namespace Models.Fighting.Battle {
    public interface IBattle {
        bool ShouldTurnEnd();

        int GetRemainingMoves(ICombatant combatant);

        int TurnNumber { get; }

        void SubmitAction(ICombatAction action);

        void EndTurn();

        bool CanMove(ICombatant combatant);

        bool CanAct(ICombatant combatant);

        List<ICombatant> GetAliveByArmy(ArmyType army);
        
        ICombatant GetById(string id);

        FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type);

        FinalizedFight FinalizeFight(FightForecast forecast);

        int GetMaxWeaponAttackRange(ICombatant combatant);

        SkillType GetWeaponSkillForRange(ICombatant combatant, int range);

        List<IObjective> GetObjectives();

        bool IsWon();

        bool IsLost();
    }
}