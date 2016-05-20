namespace Contexts.Battle.Models {
    public enum BattleUIState {
        Uninitialized,
        ContextMenu,
        SelectingUnit,
        SelectingAction,
        SelectingFightAction,
        SelectingAttackTarget,
        SelectingMoveLocation,
        ForecastingCombat,
        Fighting,
        CombatantMoving,
        PhaseChanging,
        EnemyTurn
    }
}