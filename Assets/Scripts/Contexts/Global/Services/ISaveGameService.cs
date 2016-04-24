using System.Collections.Generic;
using Contexts.Global.Models;
using Models.SaveGames;

namespace Contexts.Global.Services {
    public interface ISaveGameService {
        ISaveGame CurrentSave { get; }
        void Reset();
        void Choose(LoadedSaveGame saveGame);
        void Overwrite(ISaveGame newSave);
        List<LoadedSaveGame> GetAll();
    }
}