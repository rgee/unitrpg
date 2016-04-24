using System;
using Models.SaveGames;

namespace Contexts.Global.Models {
    public class LoadedSaveGame {
        public string Path { get; private set; } 
        public ISaveGame Save { get; private set; }

        public LoadedSaveGame(string path, ISaveGame save) {
            Path = path;
            Save = save;
        }
    }
}