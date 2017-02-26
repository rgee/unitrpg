namespace Models.Fighting.Battle {
    public interface ICombatActionVisitor {
        IPointOfInterest Visit(FightAction fight);
        IPointOfInterest Visit(MoveAction move);
        IPointOfInterest Visit(UseItemAction useItem);
    }
}