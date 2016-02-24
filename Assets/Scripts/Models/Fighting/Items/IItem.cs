namespace Models.Fighting.Items {
    public interface IItem {
        string Name { get; }
        string Id { get; }
        void Use(ICombatant target);
    }
}