using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveGames;
using UnityEngine;

public class SaveGameSaver : MonoBehaviour {
    public void OverwriteSave(State state) {
        var saveManager = new BinarySaveManager();
        saveManager.Save(state, state.Path);
        Application.LoadLevel(string.Format("chapter_{0}_intro", state.Chapter));
    }
}
