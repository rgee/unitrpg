using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Dialogue {
    public class EmotionalResponse {
        public readonly EmotionType emotion;
        public readonly Facing facing;
        public readonly int Slot;

        public EmotionalResponse(EmotionType emotion, Facing facing, int slot) {
            this.emotion = emotion;
            this.facing = facing;
            Slot = slot;
        }
    }
}
