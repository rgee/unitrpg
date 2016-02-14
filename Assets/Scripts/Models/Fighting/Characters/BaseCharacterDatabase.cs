using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public class BaseCharacterDatabase : CharacterDatabase {
        private readonly Dictionary<string, ICharacter> _characters = new Dictionary<string, ICharacter>();

        public BaseCharacterDatabase() {
            Add(new CharacterBuilder()
                .Name("Liat")
                .Stats(new StatsBuilder().Leadership().Build())
                .Weapons("Campaign Backblade", "Slim Recurve")
                .Build());

            Add(new CharacterBuilder()
                .Name("Janek")
                .Weapons("Chained Mace")
                .Build());

            Add(new CharacterBuilder()
                .Name("Maelle")
                .Weapons("Shortsword")
                .Build());
        }

        private void Add(ICharacter character) {
            _characters[character.Name] = character;
        }

        public ICharacter GetCharacter(string name) {
            return _characters[name];
        }
    }
}