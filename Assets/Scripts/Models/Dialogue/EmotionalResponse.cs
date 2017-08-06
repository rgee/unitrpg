using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Dialogue {
    public class EmotionalResponse {
        public readonly EmotionType Emotion;
        public readonly Facing Facing;
        public readonly int Slot;

        public EmotionalResponse(EmotionType emotion, Facing facing, int slot) {
            this.Emotion = emotion;
            this.Facing = facing;
            Slot = slot;
        }
    }
}
