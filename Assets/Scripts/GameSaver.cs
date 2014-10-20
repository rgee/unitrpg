using UnityEngine;
using System.Collections;

public class GameSaver : MonoBehaviour {

	public int chapter;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			SaveGame.current.chapter = chapter;
			SaveGame.Save();
		}
	}
}
