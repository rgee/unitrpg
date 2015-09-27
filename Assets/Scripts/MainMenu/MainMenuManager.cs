using SaveGames;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public void StartNewGame() {

        var saveGameSate = new State {
            Chapter = 1,
            SecondsPlayed = 0
        };

        BinarySaveManager.CurrentState = saveGameSate;
        SceneFader.Instance.TransitionToScene("chapter_2_battle");
    }

    public void ShowOptions() {
        Application.LoadLevel("Options");
    }

    public void LoadGame() {
        Application.LoadLevel("LoadGame");
    }

    public void Quit() {
        Application.Quit();
    }
}
