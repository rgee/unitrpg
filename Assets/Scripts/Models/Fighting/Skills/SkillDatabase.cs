using System;
using System.Collections.Generic;

namespace Models.Fighting.Skills {
    public class SkillDatabase {
        public ISkillStrategy GetStrategyByType(SkillType type) {
            switch (type) {
                case SkillType.Heal:
                    return new Heal();
                case SkillType.Melee:
                    return new MeleeAttack();
                case SkillType.Ranged:
                    return new ProjectileAttack();
                case SkillType.Kinesis:
                    return new Kinesis();
                case SkillType.Strafe:
                case SkillType.Advance:
                case SkillType.Overpower:
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }
    }
}