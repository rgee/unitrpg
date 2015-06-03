using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveGames;
using UnityEngine;

public class SaveGameSaver : MonoBehaviour {
    public void OverwriteSave(State state) {
        var saveManager = new BinarySaveManager();
        saveManager.Save(BinarySaveManager.CurrentState, state.Path);
        Application.LoadLevel(string.Format("chapter_{0}_intro", BinarySaveManager.CurrentState.Chapter));
    }

    public void WriteNewSave() {
        var saveManager = new BinarySaveManager();

        // TODO: Randomize path
        var path = "SaveGame.sav";
        saveManager.Save(BinarySaveManager.CurrentState, path);
        Application.LoadLevel(string.Format("chapter_{0}_intro", BinarySaveManager.CurrentState.Chapter));
    }
}
