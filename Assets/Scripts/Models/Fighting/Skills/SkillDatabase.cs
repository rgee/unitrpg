using System;
using System.Collections.Generic;
using Models.Fighting.Maps;

namespace Models.Fighting.Skills {
    public class SkillDatabase {
        private readonly IMap _map;

        public SkillDatabase(IMap map) {
            _map = map;
        }

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
                    return new Advance(new MeleeAttack(), new ProjectileAttack());
                case SkillType.Knockback:
                    return new Knockback(_map);
                default:
                    throw new ArgumentOutOfRangeException("type", type, "Invalid skill type.");
            }
        }
    }
}