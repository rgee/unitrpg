namespace Contexts.Battle.Models {
    public enum CombatStateTriggers {
        BattleStarted,
        FightAction,
        MoveAction,
        EndTurn,
        ActionsExhausted,
        ObjectiveFailed,
        AllObjectivesCompleted
    }
}