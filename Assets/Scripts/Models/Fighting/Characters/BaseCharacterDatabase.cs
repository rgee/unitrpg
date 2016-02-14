using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public class BaseCharacterDatabase : CharacterDatabase {
        private readonly Dictionary<string, ICharacter> _characters = new Dictionary<string, ICharacter>();

        public BaseCharacterDatabase() {
            var liat = new CharacterBuilder()
                .Name("Liat")
                .Weapons("Campaign Backblade", "Slim Recurve")
                .Build();
        }

        private void Add(ICharacter character) {
            _characters[character.Name] = character;
        }

        public ICharacter GetCharacter(string name) {
            return _characters[name];
        }
    }
}