namespace Contexts.Global.Models {
    public class CharacterConfiguration {
        public readonly string Id;
        public readonly string Name;

        public CharacterConfiguration(string id, string name) {
            Id = id;
            Name = name;
        }
    }
}