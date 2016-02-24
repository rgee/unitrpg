using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightPreview {
        public SkillResult Initial { get; set; }
        public SkillResult Flank { get; set; }
        public SkillResult Counter { get; set; }
        public SkillResult Double { get; set; }
        public bool AttackerDies { get; set; }
        public bool DefenderDies { get; set; }
    }
}