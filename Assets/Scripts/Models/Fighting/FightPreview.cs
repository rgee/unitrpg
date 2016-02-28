using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightPreview {
        public SkillHit Initial { get; set; }
        public SkillHit Flank { get; set; }
        public SkillHit Counter { get; set; }
        public SkillHit Double { get; set; }
        public bool AttackerDies { get; set; }
        public bool DefenderDies { get; set; }
    }
}