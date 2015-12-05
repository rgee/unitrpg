using System.Collections.Generic;
using Contexts.Global.Models;

namespace Contexts.Global.Services {
    public interface ISaveGameService {
        Models.SaveGame CurrentSave { get; }
        void Reset();
        void Choose(LoadedSaveGame saveGame);
        void Overwrite(Models.SaveGame newSave);
        List<LoadedSaveGame> GetAll();
    }
}