using System.Collections.Generic;
using Models.Fighting.Battle.Objectives;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps.Triggers;
using Models.Fighting.Skills;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Models.Fighting.Battle {
    public interface IBattle {
        bool ShouldTurnEnd();

        int GetRemainingMoves(ICombatant combatant);

        int TurnNumber { get; }

        Signal<string> EventTileSignal { get; }

        void SubmitAction(ICombatAction action);

        void EndTurn();

        bool CanMove(ICombatant combatant);

        bool CanAct(ICombatant combatant);

        List<ICombatant> GetAliveByArmy(ArmyType army);
        
        ICombatant GetById(string id);

        void SpawnCombatant(ICombatant combatant);

        FightForecast ForecastFight(ICombatant attacker, ICombatant defender, SkillType type);

        FinalizedFight FinalizeFight(FightForecast forecast);

        void ExecuteFight(FinalizedFight fight);

        int GetMaxWeaponAttackRange(ICombatant combatant);

        SkillType GetWeaponSkillForRange(ICombatant combatant, int range);

        List<IObjective> GetObjectives();

        bool IsWon();

        bool IsLost();

        void MoveCombatant(ICombatant combatant, List<Vector2> path);

        void AddEventTile(EventTile eventTile);

        void RemoveEventTile(Vector2 location);
    }
}