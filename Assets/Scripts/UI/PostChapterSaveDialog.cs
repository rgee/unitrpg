using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PostChapterSaveDialog : MonoBehaviour {
    public void GoToSaveScene() {
       Application.LoadLevel("SaveGame"); 
    }

    public void SkipSaving() {
        var chapterNumber = SaveGame.current.chapter;
        var sceneName = string.Format("chapter_{0}_intro", chapterNumber);

        Application.LoadLevel(sceneName);
    }
}
