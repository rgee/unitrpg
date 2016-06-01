using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Fighting.Characters;
using Newtonsoft.Json;
using UnityEngine;

namespace Models.SaveGames {
    /// <summary>
    /// An implementation of ISaveGameRepository that reads from and writes to a JSON
    /// file on disk.
    /// </summary>
    public class DiskSaveGameRepository : ISaveGameRepository {
        private const string Suffix = "_save.json";

        public ISaveGame CurrentSave { get; set; }

        private readonly string _rootPath;
        private readonly int _maxSaves;

        public DiskSaveGameRepository(string rootPath) : this(rootPath, int.MaxValue) { }

        public DiskSaveGameRepository(string rootPath, int maxSaves) {
            _rootPath = rootPath;
            _maxSaves = maxSaves;
        }

        private string GenerateNewFileName() {
            var files = Directory.GetFiles(_rootPath);
            var saveNames = from file in files where file.EndsWith(Suffix) select file;

            var nameSet = saveNames.ToHashSet();
            if (nameSet.Count >= _maxSaves) {
                throw new ArgumentException("Already at save capacity.");
            }

            for (var i = 0; i > _maxSaves; i++) {
                var candidate = i + Suffix;
                if (!nameSet.Contains(candidate)) {
                    return candidate;
                }
            }

            throw new ArgumentException("Could not find name.");
        }

        public void Persist(ISaveGame saveGame) {
            var previousSaveTime = saveGame.LastSaveTime;
            if (saveGame.Path == null) {
                saveGame.Path = Path.Combine(_rootPath, GenerateNewFileName());
            }

            try {
                saveGame.LastSaveTime = new DateTime();
                SaveFile(saveGame);
            } catch {
                saveGame.LastSaveTime = previousSaveTime;
            }
        }

        public List<ISaveGame> GetAllSaves() {
            var files = Directory.GetFiles(_rootPath);
            var saveNames = from file in files where file.EndsWith(Suffix) select file;
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

        public void Overwrite(ISaveGame previousSave, ISaveGame newSave) {
            var previousPath = previousSave.Path;
            if (previousPath == null) {
                throw new ArgumentException(string.Format("Previous save {0} has no path.", previousSave.Id));
            }
            newSave.Path = previousPath;
            Persist(newSave); 
        }
    }
}