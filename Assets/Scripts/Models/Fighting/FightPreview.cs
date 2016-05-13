using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightPreview {
        public SkillEffects Initial { get; set; }
        public SkillEffects Flank { get; set; }
        public SkillEffects Counter { get; set; }
        public SkillEffects Double { get; set; }
        public bool AttackerDies { get; set; }
        public bool DefenderDies { get; set; }
    }
}