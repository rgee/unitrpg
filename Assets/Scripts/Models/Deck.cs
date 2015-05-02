using System;

namespace Models {
    [Serializable]
    public class Deck {
        public Card[] cards;
        public EmotionType emotionType = EmotionType.DEFAULT;
        public string speaker;
    }
}