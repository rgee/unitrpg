using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    public void StartNewGame() {
        Application.LoadLevel(1);
    }

    public void ShowOptions() {
        
    }

    public void LoadGame() {

    }

    public void Quit() {
        Application.Quit();
    }
}
