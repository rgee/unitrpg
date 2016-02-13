using System.Collections.Generic;

namespace Models.Fighting.Skills {
    public class SkillDatabase {
        private class SkillRanges {
            public ISkillStrategy Adjacent { get; set; }
            public ISkillStrategy Ranged { get; set; }
        }

        private readonly Dictionary<string, SkillRanges> _ranges = new Dictionary<string, SkillRanges>() {
            { "Liat", new SkillRanges {
                    Adjacent = new MeleeAttack(),
                    Ranged = new ProjectileAttack()
                }
            }
        };
    }
}