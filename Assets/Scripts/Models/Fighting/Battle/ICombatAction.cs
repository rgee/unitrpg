namespace Models.Fighting.Battle {
    public interface ICombatAction {
        string GetValidationError(Turn turn);
        void Perform(Turn turn);
        IPointOfInterest GetPointofInterest(ICombatActionVisitor visitor);
    }
}