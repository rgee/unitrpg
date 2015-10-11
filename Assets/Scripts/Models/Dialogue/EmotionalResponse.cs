using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Dialogue {
    public class EmotionalResponse {
        public EmotionType emotion;
        public Facing facing;

        public EmotionalResponse(EmotionType emotion, Facing facing) {
            this.emotion = emotion;
            this.facing = facing;
        }
    }
}
