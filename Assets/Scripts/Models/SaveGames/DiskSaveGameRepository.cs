using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Fighting.Characters;

namespace Models.SaveGames {
    /// <summary>
    /// An implementation of ISaveGameRepository that reads from and writes to a JSON
    /// file on disk.
    /// </summary>
    public class DiskSaveGameRepository : ISaveGameRepository {
        public ISaveGame CurrentSave { get; set; }
        private readonly string path;

        public DiskSaveGameRepository(string path) {
            this.path = path;
        }

        public void Overwrite(ISaveGame saveGame) {
            var previousSaveTime = saveGame.LastSaveTime;
            try {
                saveGame.LastSaveTime = new DateTime();
                SaveFile(saveGame);
            } catch {
                saveGame.LastSaveTime = previousSaveTime;
            }
        }

        public List<ISaveGame> GetAllSaves() {
            var files = Directory.GetFiles(path);
            var saveNames = from file in files where file.EndsWith(".sav") select file;
            var saves = from name in saveNames select LoadFile(name);

            return saves
                .OrderBy(save => save.LastSaveTime)
                .ToList();
        }

        private static void SaveFile(ISaveGame save) {
            var stream = File.Open(save.Path, FileMode.Create);
            try {
                // TODO: write save file JSON
            } finally {
                stream.Close();
            }
        }

        private static ISaveGame LoadFile(string fileName) {
            var stream = File.Open(fileName, FileMode.Open);
            try {
                // TODO: parse save file JSON
                return null;
            } finally {
                stream.Close();
            }
        }
    }
}