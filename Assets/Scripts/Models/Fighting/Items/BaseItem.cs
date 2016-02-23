namespace Models.Fighting.Items {
    public abstract class BaseItem : IItem {
        public string Name { get; private set; }
        public string Id { get; private set; }

        protected BaseItem(string name, string id) {
            Name = name;
            Id = id;
        }

        public abstract void Use(ICombatant combatant);
    }
}