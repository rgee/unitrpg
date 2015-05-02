using UnityEngine;

public class LoadScreen : MonoBehaviour {
    // Use this for initialization
    private void Start() {
        SaveGame.Load();
    }

    private void OnGUI() {
        var idx = 0;
        foreach (var save in SaveGame.saveGames) {
            var saveGameRect = new Rect(Screen.width/2, Screen.height/2, 84, 60);
            if (GUI.Button(saveGameRect, "Save Game " + idx)) {
                Application.LoadLevel(Chapters.chapterNames[save.chapter - 1]);
            }
            idx++;
        }
    }
}