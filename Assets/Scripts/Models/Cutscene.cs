using UnityEngine;
using System.Collections;

namespace Models {
	public class Cutscene : ScriptableObject {
		public Deck[] decks;
		public CutsceneActor[] actors;
		public int nextScene;
	}
}