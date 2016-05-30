using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Fighting.Characters;
using Newtonsoft.Json;

namespace Models.SaveGames {
    /// <summary>
    /// An implementation of ISaveGameRepository that reads from and writes to a JSON
    /// file on disk.
    /// </summary>
    public class DiskSaveGameRepository : ISaveGameRepository {
        public ISaveGame CurrentSave { get; set; }
        private readonly string _rootPath;

        public DiskSaveGameRepository(string rootPath) {
            this._rootPath = rootPath;
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
            var files = Directory.GetFiles(_rootPath);
            var saveNames = from file in files where file.EndsWith(".json") select file;
            var saves = from name in saveNames select LoadFile(name);

            return saves
                .OrderBy(save => save.LastSaveTime)
                .ToList();
        }

        private static void SaveFile(ISaveGame save) {
            var json = JsonConvert.SerializeObject(save, Formatting.Indented, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All
            });
            File.WriteAllText(save.Path, json);
        }

        private static ISaveGame LoadFile(string fileName) {
            var json = File.ReadAllText(fileName);
            var result = JsonConvert.DeserializeObject<DefaultSaveGame>(json, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All
            });

            result.Path = fileName;

            return result;
        }
    }
}