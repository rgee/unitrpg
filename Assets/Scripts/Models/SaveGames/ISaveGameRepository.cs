using System;
using System.Collections.Generic;

namespace Models.SaveGames {
    public interface ISaveGameRepository {
        ISaveGame CurrentSave { get; set; }
        void Overwrite(ISaveGame saveGame);
        List<ISaveGame> GetAllSaves();
    }
}