using System.Collections.Generic;
using Models.Fighting.Characters;

namespace Models.SaveGames {
    public class TestingSaveGameRepository : ISaveGameRepository {
        public ISaveGame CurrentSave { get; set; }
        public void Overwrite(ISaveGame saveGame) {
        }

        public List<ISaveGame> GetAllSaves() {
            var characters = BaseCharacterDatabase.Instance.GetAllCharacters();
            return new List<ISaveGame> {
                new DefaultSaveGame(characters)
            };

        }
    }
}