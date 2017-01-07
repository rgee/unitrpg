using System;
using System.Collections.Generic;
using Models.Fighting.Characters;
using Newtonsoft.Json;
using Utils;

namespace Models.SaveGames {
    public class DefaultSaveGame : ISaveGame {
        public string Id { get; set; }

        [JsonIgnore]
        public string Path { get; set; }

        public int LastCompletedChapterNumber { get; set; }
        public List<ICharacter> Characters { get; set; }
        public DateTime LastSaveTime { get; set; }

        public DefaultSaveGame(List<ICharacter> characters) {
            Id = Guid.NewGuid().ToString();
            LastCompletedChapterNumber = 0;
            Characters = characters;
        }

        public ICharacter GetCharacterByName(string name) {
            return Characters.Find(character => character.Name == name);
        }
    }
}