using System;

namespace Contexts.Global.Models {
    public class LoadedSaveGame {
        public string Path { get; private set; } 
        public SaveGame Save { get; private set; }

        public LoadedSaveGame(string path, SaveGame save) {
            Path = path;
            Save = save;
        }
    }
}