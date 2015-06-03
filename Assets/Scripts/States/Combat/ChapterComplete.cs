using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveGames;
using UnityEngine;

public class ChapterComplete : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        BinarySaveManager.CurrentState.Chapter++;
        Application.LoadLevel("PostChapterSave");
    }
}
