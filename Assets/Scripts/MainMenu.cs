using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void OnGUI() {

		const int buttonHeight = 60;
		const int buttonWidth = 84;

		Rect buttonRect = new Rect(
			  Screen.width / 2  - (buttonWidth / 2),
			  (2 * Screen.height / 3) - (buttonHeight / 2),
			  buttonWidth,
			  buttonHeight
			);


		if (GUI.Button(buttonRect, "Start Game")) {
			Application.LoadLevel("Chapter 1");
		}


		Rect newGameButtonRect = new Rect(
			Screen.width / 2  - (buttonWidth / 2),
			(int)(1.5 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			);

		if (GUI.Button(newGameButtonRect, "Load Game")) {
			Application.LoadLevel("LoadGame");
		}
	}
}
