using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveGames;
using UnityEngine;

public class SaveGameLoader : MonoBehaviour {
    public void LoadChapter(State state) {
        BinarySaveManager.CurrentState = state;
        Application.LoadLevel(string.Format("chapter_{0}_intro", state.Chapter));
    }
}
