using System.Linq;
using SaveGames;
using UnityEngine;

public class ChapterComplete : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        var saveGame = BinarySaveManager.CurrentState;
        saveGame.Chapter++;

        var unitManager = CombatObjects.GetUnitManager();
        var characters = from unit in unitManager.GetFriendlies()
                         select unit.model.Character;
        saveGame.Characters = characters.ToList();

        Application.LoadLevel("PostChapterSave");
    }
}
