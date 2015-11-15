using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Contexts.Global.Models;
using UnityEngine;
using SaveGame = Contexts.Global.Models.SaveGame;

namespace Contexts.Global.Services {
    public class SaveGameService : ISaveGameService {
        public Models.SaveGame CurrentSave { get; private set; }

        private string _currentPath;

        [PostConstruct]
        public void Announce() {
            Debug.Log("constructing save game service");
        }

        public void Reset() {
            CurrentSave = new Models.SaveGame(-1, null, null);
        }

        public void Choose(LoadedSaveGame saveGame) {
            _currentPath = saveGame.Path; 
            CurrentSave = saveGame.Save;
        }

        public void Overwrite(Models.SaveGame newSave) {
            var path = _currentPath == null ? GenerateFileName() : _currentPath;
            var stream = File.Open(path, FileMode.OpenOrCreate);
            try {
                var binaryFormatter = getFormatter();
                binaryFormatter.Serialize(stream, newSave);
            } finally {
                stream.Close();
            }
        }

        private static string GenerateFileName() {
            var guid = Guid.NewGuid().ToString().Take(5);
            return guid + ".sav";
        }

        public List<LoadedSaveGame> GetAll() {
            var files = from fileName in Directory.GetFiles(Directory.GetCurrentDirectory())
                where fileName.EndsWith(".sav")
                select fileName;

            var states = from path in files select Load(path);

            return states
                .OrderBy(game => game.Save.LastSaveTime)
                .ToList();
        }

        private LoadedSaveGame Load(string path) {
            var stream = File.Open(path, FileMode.Open);
            try {
                var formatter = getFormatter();
                var save = (Models.SaveGame) formatter.Deserialize(stream);
                return new LoadedSaveGame(path, save);
            } finally {
                stream.Close();
            }
        }

        private BinaryFormatter getFormatter() {
            return new BinaryFormatter {
                Binder = new VersionDeserializationBinder()
            };
        }

        private class VersionDeserializationBinder : SerializationBinder {
            public override Type BindToType(string assemblyName, string typeName) {
                if (string.IsNullOrEmpty(assemblyName) || string.IsNullOrEmpty(typeName)) {
                    return null;
                }

                assemblyName = Assembly.GetExecutingAssembly().FullName;
                return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
            }
        }
    }
}