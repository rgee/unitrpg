namespace Contexts.Battle.Models {
    public enum BattleUIState {
        Uninitialized,
        SelectingUnit,
        SelectingAction,
        SelectingFightAction,
        SelectingAttackTarget,
        SelectingMoveLocation,
        ForecastingCombat,
        Fighting,
        CombatantMoving
    }
}