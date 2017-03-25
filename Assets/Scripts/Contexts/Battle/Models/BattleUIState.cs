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
        EventPlaying,
        ForecastingCombat,
        Fighting,
        CombatantMoving,
        PhaseChanging,
        EnemyTurn
    }
}