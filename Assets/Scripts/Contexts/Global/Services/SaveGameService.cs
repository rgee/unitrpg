using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Contexts.Global.Models;
using Models.Fighting.Characters;
using Models.SaveGames;
using UnityEngine;

namespace Contexts.Global.Services {
    public class SaveGameService : ISaveGameService {
        public ISaveGame CurrentSave { get; private set; }

        [Inject]
        public ISaveGameRepository Repository { get; set; }

        [Inject]
        public CharacterDatabase CharacterDB { get; set; }

        [PostConstruct]
        public void Announce() {
            Debug.Log("constructing save game service");
        }

        public void CreateNewGame() {
            CurrentSave = new DefaultSaveGame(CharacterDB.GetAllCharacters());
        }

        public void Choose(ISaveGame saveGame) {
            CurrentSave = saveGame;
        }

        public void Overwrite(ISaveGame newSave) {
            Repository.Overwrite(newSave);
        }

        public List<ISaveGame> GetAll() {
            return Repository.GetAllSaves();
        }
    }
}