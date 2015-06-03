using SaveGames;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public void StartNewGame() {

        var saveGameSate = new State {
            Chapter = 1,
            SecondsPlayed = 0
        };

        BinarySaveManager.CurrentState = saveGameSate;
        Application.LoadLevel("chapter_1_intro");
    }

    public void ShowOptions() {
        
    }

    public void LoadGame() {
        Application.LoadLevel("LoadGame");
    }

    public void Quit() {
        Application.Quit();
    }
}
