using UnityEngine;

public class MainMenu : MonoBehaviour {
    public void StartNewGame() {
        Application.LoadLevel("Chapter 1");
    }

    public void LoadGame() {
        Application.LoadLevel("LoadGame");
    }
}