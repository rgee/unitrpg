using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public interface CharacterDatabase {
        ICharacter GetCharacter(string name);
        List<ICharacter> GetAllCharacters();
    }
}