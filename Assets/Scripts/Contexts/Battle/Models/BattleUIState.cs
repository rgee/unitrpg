namespace Contexts.Battle.Models {
    public enum BattleUIState {
        Uninitialized,
        Preparations,
        Surveying,
        ContextMenu,
        SelectingUnit,
        SelectingAction,
        SelectingFightAction,
        SelectingAttackTarget,
        SelectingMoveLocation,
        SelectingInteractTarget,
        ForecastingCombat,
        Fighting,
        CombatantMoving,
        PhaseChanging,
        EnemyTurn
    }
}