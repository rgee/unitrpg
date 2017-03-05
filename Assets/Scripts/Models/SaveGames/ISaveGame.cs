using System;
using System.Collections.Generic;
using Models.Fighting.Characters;

namespace Models.SaveGames {
    public interface ISaveGame {
        string Id { get; set; }
        string Path { get; set;  }
        string LastSceneId { get; set; }
        List<ICharacter> Characters { get; set; }
        ICharacter GetCharacterByName(string name);
        DateTime LastSaveTime { get; set; }
    }
}