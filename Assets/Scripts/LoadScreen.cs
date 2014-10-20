using UnityEngine;
using System.Collections;

public class LoadScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SaveGame.Load ();
	}

	void OnGUI () {
		int idx = 0;
		foreach (SaveGame save in SaveGame.saveGames) {
			Rect saveGameRect = new Rect(Screen.width / 2, Screen.height/2, 84, 60);
			if (GUI.Button(saveGameRect, "Save Game " + idx)) {
				Application.LoadLevel(Chapters.chapterNames[save.chapter-1]);
			}
			idx++;
		}
	}

}
