namespace Models.Fighting.Characters {
    public interface CharacterDatabase {
        ICharacter GetCharacter(string name);
    }
}