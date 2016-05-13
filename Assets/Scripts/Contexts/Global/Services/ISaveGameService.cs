using System.Collections.Generic;
using Contexts.Global.Models;
using Models.SaveGames;

namespace Contexts.Global.Services {
    public interface ISaveGameService {
        ISaveGame CurrentSave { get; }
        void CreateNewGame();
        void Choose(ISaveGame saveGame);
        void Overwrite(ISaveGame newSave);
        List<ISaveGame> GetAll();
    }
}