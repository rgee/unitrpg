using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightPreview {
        public SkllEffects Initial { get; set; }
        public SkllEffects Flank { get; set; }
        public SkllEffects Counter { get; set; }
        public SkllEffects Double { get; set; }
        public bool AttackerDies { get; set; }
        public bool DefenderDies { get; set; }
    }
}