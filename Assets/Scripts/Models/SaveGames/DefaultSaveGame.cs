using System;
using System.Collections.Generic;
using Models.Fighting.Characters;

namespace Models.SaveGames {
    public class DefaultSaveGame : ISaveGame {
        public string Id { get; set; }
        public string Path { get; set; }
        public int ChapterNumber { get; set; }
        public List<ICharacter> Characters { get; set; }
        public DateTime LastSaveTime { get; set; }

        public DefaultSaveGame(List<ICharacter> characters) {
            Id = Guid.NewGuid().ToString();
            ChapterNumber = 0;
            Characters = characters;
        }

        public ICharacter GetCharacterByName(string name) {
            return Characters.Find(character => character.Name == name);
        }
    }
}