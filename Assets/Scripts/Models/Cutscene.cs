using UnityEngine;

namespace Models {
    public class Cutscene : ScriptableObject {
        public CutsceneActor[] actors;
        public Deck[] decks;
        public int nextScene;
    }
}