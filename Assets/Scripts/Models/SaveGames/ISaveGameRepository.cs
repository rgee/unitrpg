using System;
using System.Collections.Generic;

namespace Models.SaveGames {
    public interface ISaveGameRepository {
        ISaveGame CurrentSave { get; set; }
        void LoadSave(ISaveGame save);
        List<ISaveGame> GetAllSaves();
    }
}