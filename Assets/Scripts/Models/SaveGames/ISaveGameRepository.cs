using System;
using System.Collections.Generic;

namespace Models.SaveGames {
    public interface ISaveGameRepository {
        ISaveGame CurrentSave { get; set; }
        void Persist(ISaveGame saveGame);
        void Overwrite(ISaveGame previousSave, ISaveGame newSave);
        List<ISaveGame> GetAllSaves();
    }
}