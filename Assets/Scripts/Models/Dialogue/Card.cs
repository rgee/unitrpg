using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Dialogue {
    public class Card {
        public List<string> Lines = new List<string>();
        public EmotionType Emotion;
        public Dictionary<string, EmotionType> Emotions = new Dictionary<string, EmotionType>();
    }
}
