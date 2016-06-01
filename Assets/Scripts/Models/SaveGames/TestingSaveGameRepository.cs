using System.Collections.Generic;
using Models.Fighting.Characters;

namespace Models.SaveGames {
    public class TestingSaveGameRepository : ISaveGameRepository {
        public ISaveGame CurrentSave {
            get {
                return new DefaultSaveGame(BaseCharacterDatabase.Instance.GetAllCharacters());
            }

            set {
                
            }
        }

        public void Persist(ISaveGame saveGame) {
        }

        public List<ISaveGame> GetAllSaves() {
            var characters = BaseCharacterDatabase.Instance.GetAllCharacters();
            return new List<ISaveGame> {
                new DefaultSaveGame(characters)
            };
        }

        public void Overwrite(ISaveGame previousSave, ISaveGame newSave) {
            throw new System.NotImplementedException();
        }
    }
}