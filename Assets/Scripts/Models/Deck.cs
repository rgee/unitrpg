using UnityEngine;
using System.Collections;

namespace Models {
	[System.Serializable]
	public class Deck {
		public Card[] cards;
		public string speaker;
		public EmotionType emotionType = EmotionType.DEFAULT;
	}
}
