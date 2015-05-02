using UnityEngine;

public class GameSaver : MonoBehaviour {
    public int chapter;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            SaveGame.current.chapter = chapter;
            SaveGame.Save();
        }
    }
}